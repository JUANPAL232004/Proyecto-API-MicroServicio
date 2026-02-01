const API_URL = "https://localhost:5068/api/";

// Al cargar la página, verificamos si hay token
document.addEventListener('DOMContentLoaded', () => {
    verificarSesion();
});

function verificarSesion() {
    const token = localStorage.getItem('token');
    const viewLogin = document.getElementById('view-login');
    const viewAdmin = document.getElementById('view-admin');

    if (token) {
        viewLogin.style.display = 'none';
        viewAdmin.style.display = 'block';
        cargarProductos();
    } else {
        viewLogin.style.display = 'block';
        viewAdmin.style.display = 'none';
    }
}

// LÓGICA DE LOGIN
async function login(event) {
    event.preventDefault();
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    try {
        const response = await fetch(`${API_URL}/Auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ correoElectronico: email, password: password })
        });

        const data = await response.json();

        if (response.ok) {
            localStorage.setItem('token', data.token);
            verificarSesion();
        } else {
            alert(data.mensaje || "Credenciales incorrectas");
        }
    } catch (error) {
        alert("Error de conexión con la API");
    }
}

// LÓGICA DE PRODUCTOS (GET)
async function cargarProductos() {
    const token = localStorage.getItem('token');
    try {
        const response = await fetch(`${API_URL}/Producto`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.status === 401) logout();

        const productos = await response.json();
        const tbody = document.getElementById('lista-productos');
        tbody.innerHTML = '';

        productos.forEach(p => {
            tbody.innerHTML += `
                <tr>
                    <td>${p.idProducto}</td>
                    <td>${p.nombre}</td>
                    <td>$${p.precio}</td>
                    <td>
                        <button class="btn-edit" onclick="prepararEdicion(${p.idProducto}, '${p.nombre}', ${p.precio})">Editar</button>
                    </td>
                </tr>`;
        });
    } catch (error) {
        console.error("Error al cargar productos", error);
    }
}

function logout() {
    localStorage.removeItem('token');
    verificarSesion();
}