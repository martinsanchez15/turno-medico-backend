# 🩺 Turno Médico Backend

Este es el backend del sistema de gestión de turnos médicos desarrollado con **C# (.NET Core)** y **MongoDB**. Forma parte del proyecto final de la Tecnicatura en Desarrollo y Calidad de Software (UNSTA), dentro del proyecto de migración a **arquitectura REST**.

---

## ✅ Funcionalidades Implementadas

- Conexión con base de datos **MongoDB**
- Configuración centralizada en `appsettings.json`
- CRUD completo para **pacientes**:
  - Crear, listar, buscar por ID, editar y eliminar
- CRUD completo para **profesionales**:
  - Crear, listar, buscar por ID, editar y eliminar
  - Validación de email único
- CRUD para **turnos médicos**:
  - Crear turno
  - Listar turnos
  - Buscar por paciente o profesional
  - Eliminar turno
  - Validación de disponibilidad
- Registro y login para pacientes y profesionales:
  - **Encriptación de contraseñas** con BCrypt
  - **Validación de email**
  - **Verificación de contraseña**
  - **Generación de JWT** con `id`, `email`, `nombre`, `rol`
- Protección de rutas con **autenticación JWT**
- Control de acceso con **roles**
- Soporte de CORS para conexión con React
- Pruebas disponibles en **Swagger UI**

---

## 🛠️ Tecnologías Utilizadas

- ASP.NET Core 7
- MongoDB + MongoDB.Driver
- JWT (Json Web Tokens)
- BCrypt.Net
- Swagger (Swashbuckle)
- Git + GitHub para control de versiones

---

## 📌 Estado actual

✅ Arquitectura REST funcional y completa.  
✅ Rutas bien definidas por entidad (`/api/paciente`, `/api/profesional`, `/auth/login`).  
✅ Separación en capas (Controllers, Services, Models).  
✅ Datos protegidos y autenticación robusta.

---

## 🚀 Cómo ejecutar

1. Configurar MongoDB local o en la nube.
2. Actualizar `appsettings.json` con tu cadena de conexión.
3. Ejecutar el proyecto:
   ```bash
   dotnet run
