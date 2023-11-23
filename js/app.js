window.onload = () => {

    let stats, camera, controls, scene, parent, obj, cube, box, renderer;

    let dy = 0.01;
    let dx = 0.02;

    init();
    animate();

    function init() {

        // Kamera
        camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 1, 1000);
        camera.position.z = 5.0;

        // Ovládání
        controls = new THREE.TrackballControls(camera);
        controls.rotateSpeed = 4.0;
        controls.zoomSpeed = 1.2;
        controls.panSpeed = 0.8;
        controls.noZoom = false;
        controls.noPan = false;
        controls.staticMoving = true;
        controls.dynamicDampingFactor = 0.3;
        controls.keys = [65, 83, 68];
        controls.addEventListener("change", render);

        // Scene hierarchy
        scene = new THREE.Scene();
        parent = new THREE.Object3D();
        obj = new THREE.Object3D();
        box = new THREE.Object3D();
        parent.add(obj);
        scene.add(parent);

        // Add helper object (bounding box)
        const box_geometry = new THREE.BoxGeometry(3.01, 3.01, 1.01);
        const box_mesh = new THREE.Mesh(box_geometry, null);
        const bbox = new THREE.BoundingBoxHelper(box_mesh, 0xffffff);
        bbox.update();
        scene.add(bbox);

        // Instantiate a loader
        const loader = new THREE.TextureLoader();
        // Load a resource
        loader.load("textures/wood_texture_simple.png", // URL of texture
            (texture) => { // Function when resource is loaded
                // Create objects using texture
                const cube_geometry = new THREE.BoxGeometry(1, 1, 1);
                const tex_material = new THREE.MeshBasicMaterial({
                    map: texture
                });

                cube = new THREE.Mesh(cube_geometry, tex_material);
                obj.add(cube);

                // Call render here, because loading of texture can
                // take lot of time
                render();
            }, (xhr) => { // Function called when download progresses
                console.log((xhr.loaded / xhr.total * 100) + "% loaded");
            }, (xhr) => { // Function called when download errors
                console.log("An error happened");
            });

        // Display statistics of drawing to canvas
        stats = new Stats();
        stats.domElement.style.position = "absolute";
        stats.domElement.style.top = "0px";
        stats.domElement.style.zIndex = 100;
        document.body.appendChild(stats.domElement);

        // renderer
        renderer = new THREE.WebGLRenderer();
        renderer.setPixelRatio(window.devicePixelRatio);
        renderer.setSize(window.innerWidth, window.innerHeight);
        document.body.appendChild(renderer.domElement);

        window.addEventListener("resize", onWindowResize, false);
    }

    function onWindowResize() {
        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(window.innerWidth, window.innerHeight);
        controls.handleResize();
        render();
    }

    function animate() {
        requestAnimationFrame(animate);
        // Test of object animation
        if (cube.position.y >= 1.0 || cube.position.y <= -1.0) {
            dy = -dy;
        }
        cube.position.y += dy;
        if (obj.position.x >= 1.0 || obj.position.x <= -1.0) {
            dx = -dx;
        }
        obj.position.x += dx;
        // Update position of camera
        controls.update();
        // Render scene
        render();
    }

    function render() {
        renderer.render(scene, camera);
        // Update draw statistics
        stats.update();
    }
};
