# 🩺 Turno Médico Backend

Este es el backend del sistema de gestión de turnos médicos desarrollado con **C# (.NET Core)** y **MongoDB**. Forma parte del proyecto final de la Tecnicatura en Desarrollo y Calidad de Software (UNSTA).

---

## ✅ Funcionalidades Implementadas

- Conexión con base de datos **MongoDB**
- Configuración centralizada en `appsettings.json`
- CRUD completo para **pacientes**:
  - Crear, listar, buscar por ID, editar y eliminar
- CRUD básico para **turnos médicos**:
  - Crear turno
  - Listar todos los turnos
  - Buscar turnos por paciente
  - Eliminar turno
- Registro de pacientes con **encriptación de contraseñas** (BCrypt)
- Login de pacientes con **generación de JWT**
- Validación de rutas protegidas mediante token
- Endpoint protegido para obtener datos del paciente autenticado
- Documentación y pruebas con **Swagger**
CRUD para profesionales
---

## 🛠️ Tecnologías

- ASP.NET Core 7
- MongoDB + MongoDB.Driver
- JWT (Json Web Tokens)
- BCrypt.Net-Next
- Swagger (Swashbuckle)
- GitHub para control de versiones

---

📌 Próximas tareas a implementar

Validación de disponibilidad de turnos

Roles y permisos (paciente / profesional)

Conexión con el frontend (React)

Crear entorno de despliegue (Render / Railway)