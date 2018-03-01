
var vrDisplay;
var vrControls;
var arView;

var canvas;
var camera;
var scene;
var renderer;
var model;

var shadowMesh;
var planeGeometry;
var light;
var directionalLight;

//DATA
var data = null;
var groupedData;
var GroupTimes;


/////////////////////////////////////////////////
// HELPER METHODS
/////////////////////////////////////////////////

var dataDate = 1475193600;
var epochDay = 86400;
var pplGrp = new THREE.Object3D();

//Colors
var colors = {
    spain: 0xF26101,
    andorra: 0xFFFFFF,
    france: 0x0071BC,
    background: 0x304269,
    visitors: 0xEC4269
};

//returns obj color by nation 
function colorByNation(nation) {
    let color = null;
    if (nation === "Spain") {
        color = colors.spain;
    } else if (nation === "France") {
        color = colors.france;
    } else if (nation === "Andorra") {
        color = colors.andorra;
    } else {
        color = colors.visitors;
    }
    return color;
}

// converts lat to THREEjs close to axis points
function latCor(lat) {
    lat = ((100 * lat) - 4250);
    return lat;
}

// converts lon to THREEjs close to axis points 
function lonCor(lon) {
    lon = ((lon * 100) - 150);
    return lon;
}


/**
 * Use the `getARDisplay()` utility to leverage the WebVR API
 * to see if there are any AR-capable WebVR VRDisplays. Returns
 * a valid display if found. Otherwise, display the unsupported
 * browser message.
 */

function parseJson() {
    $.getJSON("d.json", function (data) {

        /////////////////////////////////////////////////
        //data managmemnt 
        // uses lodash `mapValues` and `groupBy`
        var groupIt = function (seq, keys) {
            if (!keys.length)
                return seq;
            var first = keys[0];
            var rest = keys.slice(1);
            return _.mapValues(_.groupBy(seq, first), function (value) {
                return groupIt(value, rest)
            });
        };
        // clone the original data obj and sort it
        // into a group of people with the same start time 
        groupedData = Object.values(groupIt(data, ['S[0].s']))
        // console.log(groupedData[1][0].S[0].la, " >> ", latCor(groupedData[1][0].S[0].la));

        // the start time of the first object in the group 
        // if this time is also the start time for a group of 
        // people's stay
        GroupTimes = Object.keys(groupIt(data, ['S[0].s']))

        console.log("loaded page and data from json");
        animPeople(groupedData);

    });
}

THREE.ARUtils.getARDisplay().then(function (display) {
    if (display) {
        vrDisplay = display;
        parseJson();
        //call THREE 
        init();
    } else {
        console.log("problemo with devico");
        THREE.ARUtils.displayUnsupportedMessage();
    }
});


function init() {
    // Turn on the debugging panel
    var arDebug = new THREE.ARDebug(vrDisplay);
    document.body.appendChild(arDebug.getElement());

    // Setup the three.js rendering environment
    renderer = new THREE.WebGLRenderer({ alpha: true });
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.autoClear = false;
    canvas = renderer.domElement;
    document.body.appendChild(canvas);
    scene = new THREE.Scene();

    // Creating the ARView, which is the object that handles
    // the rendering of the camera stream behind the three.js
    // scene
    arView = new THREE.ARView(vrDisplay, renderer);

    // The ARPerspectiveCamera is very similar to THREE.PerspectiveCamera,
    // except when using an AR-capable browser, the camera uses
    // the projection matrix provided from the device, so that the
    // perspective camera's depth planes and field of view matches
    // the physical camera on the device.
    camera = new THREE.ARPerspectiveCamera(
        vrDisplay,
        60,
        window.innerWidth / window.innerHeight,
        vrDisplay.depthNear,
        vrDisplay.depthFar
    );

    // VRControls is a utility from three.js that applies the device's
    // orientation/position to the perspective camera, keeping our
    // real world and virtual world in sync.
    vrControls = new THREE.VRControls(camera);

    // For shadows to work
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;

    // The materials in Poly models will render as a black mesh
    // without lights in our scenes. Let's add an ambient light
    // so our model can be scene, as well as a directional light
    // for the shadow
    directionalLight = new THREE.DirectionalLight();
    // @TODO in the future, use AR light estimation
    directionalLight.intensity = 0.7;
    directionalLight.position.set(10, 15, 30);
    // We want this light to cast shadow
    directionalLight.castShadow = true;
    light = new THREE.AmbientLight('white', 0.3);
    scene.add(light);
    scene.add(directionalLight);



    // Make a large plane to receive our shadows
    planeGeometry = new THREE.PlaneGeometry(2000, 2000);
    // Rotate our plane to be parallel to the floor
    planeGeometry.rotateX(-Math.PI / 2);

    // Create a mesh with a shadow material, resulting in a mesh
    // that only renders shadows once we flip the `receiveShadow` property
    shadowMesh = new THREE.Mesh(planeGeometry, new THREE.ShadowMaterial({
        color: 0x111111,
        opacity: 0.5,
    }));
    shadowMesh.receiveShadow = true;
    scene.add(shadowMesh);

    modelLoader();

    // Bind our event handlers
    window.addEventListener('resize', onWindowResize, false);
    canvas.addEventListener('click', onClick, false);

    // Kick off the render loop!
    update();
}

/**
 * The render loop, called once per frame. Handles updating
 * our scene and rendering.
 */
function update() {
    // Clears color from the frame before rendering the camera (arView) or scene.
    renderer.clearColor();

    // Render the device's camera stream on screen first of all.
    // It allows to get the right pose synchronized with the right frame.
    arView.render();

    // Update our camera projection matrix in the event that
    // the near or far planes have updated
    camera.updateProjectionMatrix();

    // Update our perspective camera's positioning
    vrControls.update();

    // Render our three.js virtual scene
    renderer.clearDepth();
    renderer.render(scene, camera);

    // Kick off the requestAnimationFrame to call this function
    // when a new VRDisplay frame is rendered
    vrDisplay.requestAnimationFrame(update);
}

/**
 * On window resize, update the perspective camera's aspect ratio,
 * and call `updateProjectionMatrix` so that we can get the latest
 * projection matrix provided from the device
 */
function onWindowResize() {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
}

/**
 * When clicking on the screen, fire a ray from where the user clicked
 * on the screen and if a hit is found, place a cube there.
 */
function onClick(e) {
    // Inspect the event object and generate normalize screen coordinates
    // (between 0 and 1) for the screen position.
    var x = e.clientX / window.innerWidth;
    var y = e.clientY / window.innerHeight;

    // Send a ray from the point of click to the real world surface
    // and attempt to find a hit. `hitTest` returns an array of potential
    // hits.
    var hits = vrDisplay.hitTest(x, y);

    if (!model) {
        console.warn('Model not yet loaded');
        return;
    }

    // If a hit is found, just use the first one
    if (hits && hits.length) {
        var hit = hits[0];

        // Turn the model matrix from the VRHit into a
        // THREE.Matrix4 so we can extract the position
        // elements out so we can position the shadow mesh
        // to be directly under our model. This is a complicated
        // way to go about it to illustrate the process, and could
        // be done by manually extracting the "Y" value from the
        // hit matrix via `hit.modelMatrix[13]`
        var matrix = new THREE.Matrix4();
        var position = new THREE.Vector3();
        matrix.fromArray(hit.modelMatrix);
        position.setFromMatrixPosition(matrix);

        // Set our shadow mesh to be at the same Y value
        // as our hit where we're placing our model
        // @TODO use the rotation from hit.modelMatrix
        shadowMesh.position.y = position.y;

        // Use the `placeObjectAtHit` utility to position
        // the cube where the hit occurred
        THREE.ARUtils.placeObjectAtHit(model,  // The object to place
            hit,   // The VRHit object to move the cube to
            1,     // Easing value from 0 to 1; we want to move
            // the cube directly to the hit position
            true);  // Whether or not we also apply orientation

        // Rotate the model to be facing the user
        var angle = Math.atan2(
            camera.position.x - model.position.x,
            camera.position.z - model.position.z
        );
        model.rotation.set(0, angle, 0);


        THREE.ARUtils.placeObjectAtHit(pplGrp,  // The object to place
            hit,   // The VRHit object to move the cube to
            1,     // Easing value from 0 to 1; we want to move
            // the cube directly to the hit position
            true);  // Whether or not we also apply orientation

        // Rotate the model to be facing the user
        var angle = Math.atan2(
            camera.position.x - pplGrp.position.x,
            camera.position.z - pplGrp.position.z
        );
        pplGrp.rotation.set(0, angle, 0);
        console.log(model.position);

    }
}
