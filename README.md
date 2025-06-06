# 🩺 Turno Médico Backend

Este es el backend del sistema de gestión de turnos médicos desarrollado con **C# (.NET Core)** y **MongoDB**. Forma parte del proyecto final de la Tecnicatura en Desarrollo y Calidad de Software (UNSTA).

---

## ✅ Funcionalidades Implementadas

- Conexión con base de datos **MongoDB**
- Configuración centralizada en `appsettings.json`
- CRUD completo para **pacientes**:
  - Crear, listar, buscar por ID, editar y eliminar
- CRUD completo para **profesionales** con protección de endpoints
- CRUD básico para **turnos médicos**:
  - Crear turno
  - Listar todos los turnos
  - Buscar turnos por paciente
  - Eliminar turno
  - Validación de disponibilidad de turnos (evita turnos duplicados en fecha/hora con el mismo profesional)
- Registro de pacientes y profesionales con **encriptación de contraseñas** (BCrypt)
- Login de pacientes y profesionales con **generación de JWT**
- Validación de rutas protegidas mediante tokens y **roles**
- Endpoint protegido para obtener datos del usuario autenticado (perfil)
- Documentación interactiva y pruebas con **Swagger**

---

## 🛠️ Tecnologías

- ASP.NET Core 7
- MongoDB + MongoDB.Driver
- JWT (Json Web Tokens)
- BCrypt.Net-Next
- Swagger (Swashbuckle)
- GitHub para control de versiones

---

## 📌 Próximas tareas a implementar

- Conexión con el frontend (React)
- Crear entorno de despliegue (Render / Railway)
