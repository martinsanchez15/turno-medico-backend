# 🩺 Turno Médico Backend

Este es el backend del sistema de gestión de turnos médicos desarrollado con **C# (.NET Core)** y **MongoDB**. Forma parte del proyecto final de la Tecnicatura en Desarrollo y Calidad de Software (UNSTA).

---

## ✅ Funcionalidades Implementadas

- Conexión con base de datos **MongoDB**
- Configuración centralizada en `appsettings.json`
- CRUD completo para **pacientes**:
  - Crear, listar, buscar por ID, editar y eliminar
- CRUD completo para **profesionales**:
  - Crear, listar, buscar por ID, editar y eliminar
  - Validación de email único
- CRUD básico para **turnos médicos**:
  - Crear turno
  - Listar todos los turnos
  - Buscar turnos por paciente
  - Eliminar turno
  - Validación de disponibilidad (no se permiten turnos duplicados en fecha/hora para un profesional)
- Registro de pacientes y profesionales con:
  - **Encriptación de contraseñas** usando BCrypt
  - **Validación de email** para evitar duplicados
- Login para pacientes y profesionales con:
  - **Verificación de contraseña**
  - **Generación de JWT** con Claims personalizados
- Rutas protegidas mediante autenticación **JWT**
- Control de acceso mediante **roles** (Paciente / Profesional)
- Endpoints protegidos para obtener el perfil del usuario autenticado (`/api/paciente/perfil` y `/api/profesional/perfil`)
- Permite probar todas las rutas desde **Swagger UI**
- Soporte de CORS habilitado para permitir conexión con frontend React

---

## 🛠️ Tecnologías Utilizadas

- ASP.NET Core 7
- MongoDB + MongoDB.Driver
- JWT (Json Web Tokens)
- BCrypt.Net-Next
- Swagger (Swashbuckle.AspNetCore)
- Git + GitHub para control de versiones

---

## 📌 Próximas Tareas a Implementar

- Conexión completa con el **frontend en React**
- Agregar validaciones más detalladas en los modelos
- Crear entorno de despliegue (Render / Railway)
- Configuración para entornos de producción y testing

---

## 🔐 Autenticación y Roles

Los usuarios (pacientes y profesionales) pueden autenticarse vía `/auth/login`, y obtendrán un token JWT. Este token debe incluirse en el header `Authorization` como:

