// Asegúrate de que este archivo se llame Register.js y esté en la carpeta js/
document.addEventListener('DOMContentLoaded', () => {
    const registerForm = document.getElementById('register-form');

    if (registerForm) {
        registerForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const nombre = document.getElementById('reg-name').value;
            const email = document.getElementById('reg-email').value;
            const password = document.getElementById('reg-password').value;
            const confirmPassword = document.getElementById('confirm-password').value;
            const errorMsg = document.getElementById('reg-error-message');

            // Validar contraseñas iguales en el cliente
            if (password !== confirmPassword) {
                errorMsg.innerText = "Las contraseñas no coinciden.";
                errorMsg.style.display = 'block';
                return;
            }


            const datos = {
                NombreUsuario: nombre,
                CorreoElectronico: email,
                Password: password
            };

            try {
                // 3. Llamada al AuthController
                const response = await fetch('http://localhost:5068/api/auth/registrar', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(datos)
                });

                const result = await response.json();

                if (response.ok) {
                    alert("Usuario registrado con éxito");
                    window.location.href = 'index.html'; 
                } else {
                    errorMsg.innerText = result.mensaje || "Error al registrar";
                    errorMsg.style.display = 'block';
                }
            } catch (error) {
                console.error("Error en la petición:", error);
                errorMsg.innerText = "No se pudo conectar con el servidor.";
                errorMsg.style.display = 'block';
            }
        });
    }
});