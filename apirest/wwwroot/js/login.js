document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('login-form');

    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            const datos = {
                CorreoElectronico: document.getElementById('email').value,
                Password: document.getElementById('password').value
            };

            try {
                // Ruta: api/auth/login (Como está en tu AuthController)
                const response = await fetch('http://localhost:5068/api/auth/login', { // Cambiado a /login
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ 
                    CorreoElectronico: email, // Asegúrate que coincida con tu LoginRequest.cs
                    Password: password 
                })
                });

                const result = await response.json();

                if (response.ok) {
                    const data = await response.json();
                    
                    // Guardamos los datos que envía tu controlador
                    localStorage.setItem('token', data.token); 
                    localStorage.setItem('usuario', data.usuario);

                    alert(data.mensaje); // Dirá "Login exitoso"
                    window.location.href = "productos.html"; 
                    } else {
                    const errorElement = document.getElementById('error-message');
                    errorElement.innerText = result.mensaje || "Credenciales incorrectas";
                    errorElement.style.display = 'block';
                }
            } catch (error) {
                console.error("Error:", error);
                alert("Error de conexión con el servidor");
            }
        });
    }
});