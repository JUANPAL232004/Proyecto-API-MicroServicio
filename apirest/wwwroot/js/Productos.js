const API_URL = 'http://localhost:5068/api/producto'; 
const token = localStorage.getItem('token'); 

let paginaActual = 1;
const productosPorPagina = 20; 
let listaCompleta = []; 

document.addEventListener('DOMContentLoaded', () => {
    if (!token) { 
        window.location.href = 'index.html'; 
        return; 
    }
    obtenerProductos();
});

// 1. Obtener lista de productos
async function obtenerProductos() {
    try {
        const response = await fetch(API_URL, {
            headers: { 
                'Authorization': `Bearer ${token}`, 
                'Content-Type': 'application/json' 
            }
        });

        if (response.ok) {
            listaCompleta = await response.json();
            renderizarPagina(paginaActual);
        } else {
            const errorMsg = await detallarError(response);
            alert(`Error al cargar productos: ${errorMsg}`);
            if (response.status === 401) logout();
        }
    } catch (error) { 
        console.error("Error de conexión:", error);
        alert("No hay conexión con el servidor. ¿El backend está activo?");
    }
}

// 2. Función para leer errores detallados del servidor
async function detallarError(response) {
    let mensaje = `Código: ${response.status}`;
    try {
        const data = await response.json();
        if (data.errors) {
            mensaje += "\nErrores:";
            for (const key in data.errors) {
                mensaje += `\n- ${key}: ${data.errors[key].join(', ')}`;
            }
        } else if (data.message) {
            mensaje += `\nDetalle: ${data.message}`;
        }
    } catch (e) {
        mensaje += ` (${response.statusText})`;
    }
    return mensaje;
}

// 3. Renderizar tabla con inputs directamente en las celdas (Edición Inline)
function renderizarPagina(pagina) {
    paginaActual = pagina;
    const tbody = document.getElementById('tabla-productos');
    if (!tbody) return;
    tbody.innerHTML = '';

    const inicio = (paginaActual - 1) * productosPorPagina;
    const fin = inicio + productosPorPagina;
    const bloquePagina = listaCompleta.slice(inicio, fin);

    bloquePagina.forEach(p => {
    const tr = document.createElement('tr');
    tr.id = `fila-${p.idProducto}`;
    
    // Mostramos el NombreEstado como texto y el IdEstado oculto para las actualizaciones
    tr.innerHTML = `
        <td>${p.idProducto}</td>
        <td><input type="text" class="input-inline" id="n-${p.idProducto}" value="${p.nombreProducto || ''}"></td>
        <td><input type="text" class="input-inline" id="c-${p.idProducto}" value="${p.cliente || ''}"></td>
        <td><input type="number" class="input-inline" id="p-${p.idProducto}" value="${p.precio}" step="0.01"></td>
        <td><input type="number" class="input-inline" id="s-${p.idProducto}" value="${p.stock}"></td>
        
        <td>
    <input type="hidden" id="e-${p.idProducto}" value="${p.idEstado}">
    <span class="badge-estado" style="font-weight: bold; color: #007bff;">
        ${p.nombreEstado || "Cargando..."} 
    </span>
</td>
        <td><small>${p.fecha ? new Date(p.fecha).toLocaleDateString() : 'N/A'}</small></td>
        <td>
            <button class="btn-actualizar" onclick="guardarCambiosInline(${p.idProducto})">Actualizar</button>
            <button class="btn-delete" onclick="eliminarProducto(${p.idProducto})">Eliminar</button>
        </td>
    `;
    tbody.appendChild(tr);
});
    actualizarControlesPaginacion();
}

// 4. Guardar cambios (PUT) capturando los datos directamente de la fila
async function guardarCambiosInline(id) {
    const editado = {
        IdProducto: id,
        NombreProducto: document.getElementById(`n-${id}`).value.trim(),
        Cliente: document.getElementById(`c-${id}`).value.trim(),
        Precio: parseFloat(document.getElementById(`p-${id}`).value),
        Stock: parseInt(document.getElementById(`s-${id}`).value),
        IdEstado: parseInt(document.getElementById(`e-${id}`).value)
    };

    if (!editado.NombreProducto || isNaN(editado.Precio)) {
        alert("Nombre y Precio son obligatorios.");
        return;
    }

    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: 'PUT',
            headers: { 
                'Authorization': `Bearer ${token}`, 
                'Content-Type': 'application/json' 
            },
            body: JSON.stringify(editado)
        });

        if (response.ok) {
            // Feedback visual: la fila se ilumina en verde brevemente
            const fila = document.getElementById(`fila-${id}`);
            fila.classList.add('fila-guardada');
            setTimeout(() => fila.classList.remove('fila-guardada'), 1500);
            
            // Sincronizar la lista en memoria para no perder cambios al cambiar de página
            const idx = listaCompleta.findIndex(x => x.idProducto === id);
            if (idx !== -1) {
                listaCompleta[idx] = { 
                    ...listaCompleta[idx], 
                    nombreProducto: editado.NombreProducto,
                    cliente: editado.Cliente,
                    precio: editado.Precio,
                    stock: editado.Stock,
                    idEstado: editado.IdEstado
                };
            }
            console.log(`Producto ${id} actualizado.`);
        } else {
            const msjError = await detallarError(response);
            alert(`Error al guardar:\n${msjError}`);
        }
    } catch (error) { 
        alert("Error crítico de comunicación con el servidor.");
    }
}

// 5. Eliminar producto
async function eliminarProducto(id) {
    if (!confirm(`¿Desea eliminar el producto ID ${id}?`)) return;

    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            listaCompleta = listaCompleta.filter(p => p.idProducto !== id);
            renderizarPagina(paginaActual);
        } else {
            alert("No se pudo eliminar el producto.");
        }
    } catch (error) { console.error("Error:", error); }
}

// 6. Controles de Paginación
function actualizarControlesPaginacion() {
    const contenedor = document.getElementById('paginacion-controles');
    if (!contenedor) return;
    const totalPaginas = Math.ceil(listaCompleta.length / productosPorPagina) || 1;
    
    contenedor.innerHTML = `
        <button ${paginaActual === 1 ? 'disabled' : ''} onclick="renderizarPagina(${paginaActual - 1})">« Anterior</button>
        <span style="margin: 0 15px">Página <b>${paginaActual}</b> de ${totalPaginas}</span>
        <button ${paginaActual === totalPaginas ? 'disabled' : ''} onclick="renderizarPagina(${paginaActual + 1})">Siguiente »</button>
    `;
}

function logout() {
    localStorage.removeItem('token');
    window.location.href = 'index.html';
}