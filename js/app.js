window.onload = () => {

    let stats, camera, controls, scene, renderer, obj_box, obj_pad;

    let dy = 0.01;
    let dx = 0.02;

    init();
    animate();

    function init() {
        // Texture loader
        const loader = new THREE.TextureLoader();

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

        // Add helper object (bounding box)
        const box_geometry = new THREE.BoxGeometry(3.01, 3.01, 1.01);
        const box_mesh = new THREE.Mesh(box_geometry, null);
        const bounding_box = new THREE.BoundingBoxHelper(box_mesh, 0xffffff);
        bounding_box.update();
        scene.add(bounding_box);

        // obj_box
        loader.load("textures/wood_texture_simple.png", (texture) => {
            const geometry = new THREE.BoxGeometry(1, 1, 1);
            const material = new THREE.MeshBasicMaterial({map: texture});
            obj_box = new THREE.Mesh(geometry, material);
            scene.add(obj_box);
        });

        // obj_pad
        loader.load("textures/wood_texture_simple.png", (texture) => {
            const geometry = new THREE.CylinderGeometry(.2, .2, 2.5, 10);
            const material = new THREE.MeshBasicMaterial({map: texture});
            obj_pad = new THREE.Mesh(geometry, material);
            scene.add(obj_pad);
        });

        // Display statistics of drawing to canvas
        stats = new Stats();
        stats.domElement.style.position = "absolute";
        stats.domElement.style.top = "0px";
        stats.domElement.style.zIndex = "100";
        document.body.appendChild(stats.domElement);

        // Renderer
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
        if (obj_box.position.y >= 1.0 || obj_box.position.y <= -1.0) {
            dy = -dy;
        }
        obj_box.position.y += dy;
        if (obj_box.position.x >= 1.0 || obj_box.position.x <= -1.0) {
            dx = -dx;
        }
        obj_box.position.x += dx;
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
