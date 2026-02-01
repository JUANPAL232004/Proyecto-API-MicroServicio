// 1. Configuración Base
const API_URL = "http://localhost:5068/api";

// Variables para Paginación
let paginaActual = 1;
const productosPorPagina = 20; // <--- AJUSTADO A 20 LEADS POR PÁGINA
let listaCompleta = []; 

document.addEventListener('DOMContentLoaded', () => {
    if (window.location.pathname.includes('productos.html')) {
        cargarProductos();
    }
    
    const loginForm = document.getElementById('login-form');
    if (loginForm) {
        loginForm.addEventListener('submit', ejecutarLogin);
    }
});

// 2. Lógica de Login
async function ejecutarLogin(e) {
    e.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ CorreoElectronico: email, Password: password })
        });
        const data = await response.json();
        if (response.ok) {
            localStorage.setItem('token', data.token);
            localStorage.setItem('usuario', data.usuario);
            window.location.href = 'productos.html';
        } else {
            alert(data.mensaje || "Credenciales incorrectas");
        }
    } catch (error) {
        alert("Error de conexión");
    }
}

// 3. Lógica de Productos con Paginación
async function cargarProductos() {
    const token = localStorage.getItem('token');
    if (!token) { window.location.href = 'index.html'; return; }

    try {
        const response = await fetch(`${API_URL}/producto`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            listaCompleta = await response.json();
            // Al recargar o cargar por primera vez, vamos a la página 1
            mostrarPagina(1);
        } else if (response.status === 401) {
            logout();
        }
    } catch (error) {
        console.error("Error al cargar productos:", error);
    }
}

// Función para renderizar solo una parte de la lista (Límite de 20)
function mostrarPagina(pagina) {
    paginaActual = pagina;
    const tbody = document.getElementById('tabla-productos');
    if (!tbody) return;

    // LIMPIAMOS LA TABLA para que no se acumulen los datos
    tbody.innerHTML = '';

    // Lógica de recorte de array para 20 elementos
    const inicio = (paginaActual - 1) * productosPorPagina;
    const fin = inicio + productosPorPagina;
    const productosAMostrar = listaCompleta.slice(inicio, fin);

    productosAMostrar.forEach(p => {
        const productoJson = JSON.stringify(p).replace(/"/g, '&quot;');
        tbody.innerHTML += `
            <tr>
                <td>${p.idProducto}</td>
                <td>${p.nombreProducto}</td>
                <td>${p.cliente}</td>
                <td>$${p.precio}</td>
                <td>${p.stock}</td>
                <td>${p.idEstado}</td>
                <td>${new Date(p.fecha).toLocaleDateString()}</td>
                <td>
                    <button class="btn-edit" onclick="prepararEdicion(${productoJson})">Actualizar</button>
                    <button class="btn-delete" onclick="eliminarProducto(${p.idProducto})">Eliminar</button>
                </td>
            </tr>`;
    });

    renderizarControlesPaginacion();
}

// Genera los botones de páginas
function renderizarControlesPaginacion() {
    const contenedor = document.getElementById('paginacion-controles');
    if (!contenedor) return;

    const totalPaginas = Math.ceil(listaCompleta.length / productosPorPagina);
    let botonesHtml = '';

    // Botón Anterior
    botonesHtml += `<button ${paginaActual === 1 ? 'disabled' : ''} onclick="mostrarPagina(${paginaActual - 1})">Anterior</button>`;

    // Botones de números (limitamos visualmente si son demasiados, pero aquí mostramos todos)
    for (let i = 1; i <= totalPaginas; i++) {
        botonesHtml += `<button class="${i === paginaActual ? 'active' : ''}" onclick="mostrarPagina(${i})">${i}</button>`;
    }

    // Botón Siguiente
    botonesHtml += `<button ${paginaActual === totalPaginas ? 'disabled' : ''} onclick="mostrarPagina(${paginaActual + 1})">Siguiente</button>`;

    contenedor.innerHTML = botonesHtml;
}

// 4. Funciones de Edición, Guardado y Eliminación
function prepararEdicion(p) {
    // Rellenamos el formulario con los datos del objeto 'p'
    document.getElementById('prod-id').value = p.idProducto;
    document.getElementById('prod-nombre').value = p.nombreProducto;
    document.getElementById('prod-cliente').value = p.cliente;
    document.getElementById('prod-precio').value = p.precio;
    document.getElementById('prod-stock').value = p.stock;
    document.getElementById('prod-estado').value = p.idEstado;
    
    const btn = document.querySelector('.btn-save');
    if(btn) btn.innerText = "Confirmar Actualización";
}

async function guardarProducto() {
    const token = localStorage.getItem('token');
    const id = document.getElementById('prod-id').value;

    const producto = {
        NombreProducto: document.getElementById('prod-nombre').value,
        Cliente: document.getElementById('prod-cliente').value,
        Precio: parseFloat(document.getElementById('prod-precio').value),
        Stock: parseInt(document.getElementById('prod-stock').value),
        IdEstado: parseInt(document.getElementById('prod-estado').value)
    };

    const metodo = id ? 'PUT' : 'POST';
    const url = id ? `${API_URL}/producto/${id}` : `${API_URL}/producto`;
    if (id) producto.IdProducto = parseInt(id);

    try {
        const response = await fetch(url, {
            method: metodo,
            headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
            body: JSON.stringify(producto)
        });

        if (response.ok) {
            alert(id ? "Producto actualizado correctamente" : "Producto creado correctamente");
            limpiarFormulario();
            cargarProductos(); // Esto refresca la lista completa y vuelve a paginar
        } else {
            alert("Error al procesar la solicitud");
        }
    } catch (error) { 
        console.error("Error en guardarProducto:", error); 
    }
}

async function eliminarProducto(id) {
    const token = localStorage.getItem('token');
    if (!confirm("¿Estás seguro de que deseas eliminar este producto?")) return;
    try {
        const response = await fetch(`${API_URL}/producto/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (response.ok) {
            alert("Producto eliminado");
            cargarProductos();
        }
    } catch (error) { 
        console.error("Error en eliminarProducto:", error); 
    }
}

function limpiarFormulario() {
    document.getElementById('prod-id').value = "";
    document.getElementById('prod-nombre').value = "";
    document.getElementById('prod-cliente').value = "";
    document.getElementById('prod-precio').value = "";
    document.getElementById('prod-stock').value = "";
    document.getElementById('prod-estado').value = "";
    const btn = document.querySelector('.btn-save');
    if(btn) btn.innerText = "Guardar Producto";
}

function logout() {
    localStorage.clear();
    window.location.href = 'index.html';
}