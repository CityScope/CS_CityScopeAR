

function animPeople(groupedData) {
	console.log("adding people to scene");

	//spwan empty grp at start 
	scene.add(pplGrp);

	//THREE static vars 
	var pplTexture = new THREE.TextureLoader().load("img/lf4.png");

	//delay loop for spawning ppl groups 
	for (var j = 0; j <= groupedData.length; j++) {
		(function (i) {
			setTimeout(function () {
				//call drwaing method
				drawGrp(i)
				// the delay time times the iterator 
			}, ((GroupTimes[i] - dataDate) * i) / 10);
		})(j);
	}

	// the drawing function itself
	function drawGrp(i) {
		let grp = groupedData[i];
		//fix nulled group 
		if (grp) {
			for (let person = 0; person < grp.length; person++) {
				var spriteMaterial = new THREE.SpriteMaterial({
					map: pplTexture,
					transparent: true
				});
				var pplSprite = new THREE.Sprite(spriteMaterial);
				pplSprite.material.color.setHex(colorByNation(grp[person].N));
				// pplSprite.material.blending = THREE.AdditiveBlending;
				pplSprite.material.transparent = true;
				pplSprite.scale.set(.05, .05, .05);

				//location of first known pnt of this peron in this gorup  // (grp[person].S[0].s - dataDate) / 600,
				pplSprite.position.set(latCor(grp[person].S[0].la), 0, lonCor(grp[person].S[0].lo));

				// lower opacity for andorra 
				if (grp[person].N != "Andorra") {
					pplSprite.material.opacity = 1;
				} else {
					pplSprite.material.opacity = 0.25;
				}

				//add this person to scene 
				pplGrp.add(pplSprite);

				//pass stay events of person and it's visual rep. to anim method
				animStays(pplSprite, Object.values(grp[person].S));

			}
		}
	}
}

function animStays(obj, personStayEvents) {
	if (personStayEvents.length > 0) {
		for (let i = 0; i < personStayEvents.length; i++) {
			var tween = new TWEEN.Tween(obj.position).to({
				x: latCor(personStayEvents[i].la),
				y: 0,
				z: lonCor(personStayEvents[i].lo)
			}, personStayEvents[i].l).start();
		}
	}
}
