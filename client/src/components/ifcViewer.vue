<template>
    <div>
      <h2>IFC Model Viewer</h2>
      <!-- The canvas where Three.js renders the model will go here -->
      <div ref="canvasContainer"></div>
      <input type="file" @change="handleFileUpload" />
    </div>
  </template>
  
  <script setup>
  import * as THREE from 'three';
  import { onMounted, ref } from 'vue';
  import { IFCLoader } from 'three/examples/jsm/loaders/IFCLoader.js'; // Import IFCLoader
  import { GLTFExporter } from 'three/examples/jsm/exporters/GLTFExporter.js'; // Import GLTFExporter
  import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'; // Import OrbitControls
  
  // Reference for the container where the model will be rendered
  const canvasContainer = ref(null);
  
// Three.js setup for the scene
let scene, camera, renderer, loader, controls, raycaster, mouse;
  
// Initialize raycaster and mouse
raycaster = new THREE.Raycaster();
mouse = new THREE.Vector2();

onMounted(() => {
  // Set up the scene, camera, and renderer
  scene = new THREE.Scene();
  camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
  renderer = new THREE.WebGLRenderer({ antialias: true });

  // Set renderer size and append to the canvas container
  renderer.setSize(window.innerWidth, window.innerHeight);
  canvasContainer.value.appendChild(renderer.domElement);

  // Camera position
  camera.position.set(0, 2, 10);
  camera.lookAt(0, 0, 0);

  // Add lighting to the scene
  const light = new THREE.DirectionalLight(0xffffff, 1);
  light.position.set(10, 10, 10);
  scene.add(light);

  const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
  scene.add(ambientLight);

  // Initialize the IFC loader
  loader = new IFCLoader();
  loader.ifcManager.setWasmPath('/');

  // Initialize OrbitControls
  controls = new OrbitControls(camera, renderer.domElement);
  controls.enableDamping = true;
  controls.dampingFactor = 0.05;
  controls.screenSpacePanning = false;
  controls.minDistance = 1;
  controls.maxDistance = 50;

  // Add click event listener
  renderer.domElement.addEventListener('click', onClick);

  // Render the scene
  animate();
});
  
// Animation loop
function animate() {
  requestAnimationFrame(animate);

  // Update controls
  controls.update();

  // Render the scene from the camera's perspective
  renderer.render(scene, camera);
}
  
  // Handle file upload
  const handleFileUpload = (event) => {
    const file = event.target.files[0];
    if (file) {
      // Load the selected IFC file
      loadIfcModel(file);
    }
  };
  
  // Load the IFC model and convert it to GLTF
  const loadIfcModel = (file) => {
  const reader = new FileReader();
  reader.onload = async (e) => {
    const ifcData = e.target.result;

    try {
      // Parse the IFC file and add it to the scene
      const model = await loader.parse(ifcData);

      // Scale the model down
      model.scale.set(0.1, 0.1, 0.1); // Adjust the scale values as needed

      // Add the model to the scene
      scene.add(model);

      // Convert the IFC model to GLTF
      convertToGLTF(model);
    } catch (error) {
      console.error('Error parsing IFC file:', error);
    }
  };
  reader.readAsArrayBuffer(file);
};
  
  // Convert the IFC model to GLTF
  const convertToGLTF = (model) => {
    const exporter = new GLTFExporter();
    exporter.parse(
      model,
      (gltf) => {
        // Save the GLTF file or use it in the scene
        const blob = new Blob([JSON.stringify(gltf)], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
  
        // Download the GLTF file
        const link = document.createElement('a');
        link.href = url;
        link.download = 'model.gltf';
        link.click();
  
        console.log('GLTF file created:', gltf);
      },
      (error) => {
        console.error('Error exporting GLTF:', error);
      }
    );
  };
  
  // Handle click event to get coordinates
function onClick(event) {
  // Calculate mouse position in normalized device coordinates (-1 to +1)
  const rect = renderer.domElement.getBoundingClientRect();
  mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
  mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

  // Set raycaster from camera and mouse position
  raycaster.setFromCamera(mouse, camera);

  // Check for intersections with objects in the scene
  const intersects = raycaster.intersectObjects(scene.children, true);

  if (intersects.length > 0) {
    const point = intersects[0].point; // Get the intersection point
    console.log('Clicked coordinates:', point.x, point.y, point.z);
  }
}
  </script>
  
  <style scoped>
  /* Optional: you can style the div container here if needed */
  </style>