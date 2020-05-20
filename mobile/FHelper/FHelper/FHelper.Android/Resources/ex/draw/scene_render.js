export function renderScene(domWrapper, style) {
	let scene = new THREE.Scene();
	scene.background = new THREE.Color(0xFFFFFF);
					
	let renderer = new THREE.WebGLRenderer({
		precision : "heighp",
		antialias : true
	});
	
	renderer.shadowMap.enabled = true;
	renderer.shadowMap.type = THREE.PCFSoftShadowMap;
	renderer.setSize(domWrapper.offsetWidth, domWrapper.offsetHeight);
	renderer.domElement.classList.add(style);
	domWrapper.appendChild(renderer.domElement);
					
	let camera = new THREE.PerspectiveCamera(20, domWrapper.offsetWidth / domWrapper.offsetHeight, 0.1, 10000);
	camera.position.set(600, 400, 600);
	camera.lookAt(new THREE.Vector3(0, 0, 0));
	
	let light = new THREE.PointLight(0xFFFFFF, 1.2);
	light.position.set(150, 150, 150);
	light.castShadow = true;
	scene.add(light);
						
	let bottomPlaneGeometry = new THREE.PlaneGeometry(500, 500).rotateX(-Math.PI / 2).translate(0, -150, 0);
	let bottomPlaneMaterial = new THREE.MeshPhysicalMaterial( { color: 0xFFFFFF } );
	let bottomPlane = new THREE.Mesh(bottomPlaneGeometry, bottomPlaneMaterial);
	scene.add(bottomPlane);
						
	let backPlaneGeometry = new THREE.PlaneGeometry(500, 500).translate(0, 0, -150);
	let backPlaneMaterial = new THREE.MeshPhysicalMaterial( { color: 0xEEEEEE } );
	let backPlane = new THREE.Mesh(backPlaneGeometry, backPlaneMaterial);
	scene.add(backPlane);
						
	let leftPlaneGeometry = new THREE.PlaneGeometry(500, 500).rotateY(Math.PI / 2).translate(-150, 0, 0);
	let leftPlaneMaterial = new THREE.MeshPhysicalMaterial( { color: 0xDDDDDD } );
	let leftPlane = new THREE.Mesh(leftPlaneGeometry, leftPlaneMaterial);
	scene.add(leftPlane);
	
	return {
		scene : scene,
		renderer : renderer,
		camera : camera
	};
}