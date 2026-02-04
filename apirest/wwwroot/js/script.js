const API_URL = "https://localhost:7123/api/producto"; 

async function obtenerProductos() {
    try {
        const response = await fetch(API_URL);
        const productos = await response.json();
        
        const tabla = document.getElementById("tablaProductos");
        tabla.innerHTML = "";

        productos.forEach(p => {
            tabla.innerHTML += `
                <tr>
                    <td>${p.idProducto}</td>
                    <td>${p.nombreProducto}</td>
                    <td>${p.precio}</td>
                    <td>${p.nombreEstado}</td> 
                    <td>
                        <button onclick="eliminar(${p.idProducto})">Eliminar</button>
                    </td>
                </tr>
            `;
        });
    } catch (error) {
        console.error("Error al obtener productos:", error);
    }
}

async function guardarProducto() {
    const nuevoProducto = {
        nombreProducto: document.getElementById("nombre").value,
        precio: parseFloat(document.getElementById("precio").value),
        cliente: document.getElementById("cliente").value,
        stock: parseInt(document.getElementById("stock").value),
        idEstado: parseInt(document.getElementById("idEstado").value)
    };

    const response = await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(nuevoProducto)
    });

    if (response.ok) {
        alert("Producto creado con éxito");
        obtenerProductos();
    } else {
        const errorData = await response.json();
        alert("Error: " + errorData.message);
    }
}