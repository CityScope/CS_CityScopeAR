function modelLoader() {

    //model loader
    var loader = new THREE.ObjectLoader();

    console.log("Loading Model")
    var prgsDiv = document.createElement('div');
    prgsDiv.setAttribute("id", "prgsDiv");
    document.body.appendChild(prgsDiv);

    loader.load(
        // resource URL
        'model/Andorra.json',
        // called when resource is loaded
        function (m) {
            m.scale.set(1, 1, 1)
            m.position.set(0, 0, 0)
            m.children.forEach(function (mesh) {
                mesh.castShadow = true;
            });
            model = m;
            scene.add(model);
            console.log("-- Model loading is done --")
        },
        // called when loading is in progresses
        function (xhr) {
            // console.log(xhr.loaded + " >>Still Loading>>");
        },
        // called when loading has errors
        function (error) {
            console.log('loading model error happened');
        }
    );
}