# 🎵 MelodyMuse

A modern, full-stack web application for music discovery, management, and exploration. Built with Vue 3 on the frontend and ASP.NET Core on the backend.

## 📋 Project Overview

**MelodyMuse** is a collaborative music platform project combining cutting-edge frontend technologies with a robust backend infrastructure. The project demonstrates a complete full-stack development workflow with multiple contributors working across frontend (Vue/JavaScript) and backend (C#/.NET) components.

### Key Metrics
- **Repository Created**: July 8, 2024
- **Language Composition**: 
  - Vue: 51.2%
  - C#: 44.8%
  - JavaScript: 3.5%
  - Other: 0.5%
- **Status**: Active Development
- **Contributors**: Multiple team members collaborating on features

## 🏗️ Project Architecture

### Frontend Structure
```
melodymuse.client/
├── src/
│   ├── main.js           # Application entry point
│   ├── App.vue           # Root component
│   ├── router/           # Vue Router configuration
│   ├── store/            # Vuex state management
│   └── components/       # Vue components
└── package.json          # Frontend dependencies
```

### Backend Structure
```
MelodyMuse.Server/
├── Controllers/          # API endpoints
├── Services/             # Business logic
├── Models/               # Data models
└── Configuration/        # Server setup
```

## 🛠️ Technology Stack

### Frontend
- **Vue 3.4.29** - Progressive JavaScript framework
- **Vite 5.4.1** - Next-generation frontend build tool
- **Vue Router 4.4.0** - Client-side routing
- **Vuex 4.0.2** - State management
- **Element Plus 2.7.7** - UI component library
- **Axios 1.7.2** - HTTP client for API communication
- **Pinyin Support** - Chinese character/pinyin conversion libraries
  - `pinyin: 4.0.0-alpha.2`
  - `tiny-pinyin: 1.3.2`
- **Sass 1.77.8** - CSS preprocessing
- **ESLint** - Code quality and consistency

### Backend
- **ASP.NET Core** - Modern .NET web framework
- **C#** - Primary backend language
- Built with MelodyMuse.sln solution structure

### Development Tools
- **ESLint 8.57.0** - JavaScript/Vue linting
- **VS Code** - Configured development environment (`.vscode/` settings included)
- **Git** - Version control with comprehensive .gitignore

## 📦 Installation & Setup

### Prerequisites
- Node.js (latest LTS)
- npm or yarn
- .NET SDK (for backend development)
- Visual Studio or VS Code with appropriate extensions

### Frontend Setup
```bash
cd melodymuse.client
npm install
```

### Development Server
```bash
# Start frontend development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Lint and fix code
npm run lint
```

### Backend Setup
```bash
# Open solution in Visual Studio
# Or use dotnet CLI
dotnet restore
dotnet build
dotnet run
```

## 🔄 Development Process & Workflow

### Project Timeline
The project has evolved through several development phases:

1. **Initial Foundation** (July - August 2024)
   - Project setup and architecture planning
   - Repository initialization by @saikewei
   - Initial configuration and tooling

2. **Early Development** (August - September 2024)
   - @Fubbrr joined the team with multiple commits
   - Core component development
   - Feature implementation sprint

3. **Backend Integration** (September - October 2024)
   - Backend API development
   - Integration of ASP.NET Core services
   - Database schema design and implementation
   - Contributors: @ChenYX510, @wukdhxbsdkjxn

4. **Enhancement Phase** (October - December 2024)
   - Frontend refinement and optimization
   - API endpoint polishing
   - Chinese language support implementation
   - Code quality improvements
   - Multiple iterations by @CoArrayLiu

5. **Recent Updates** (December 2025 onwards)
   - Ongoing feature enhancements
   - Performance optimization
   - Continued collaboration and refinement

### Commit History Insights
- **Multiple Contributors**: Collaborative development with contributions from:
  - @saikewei (Project lead)
  - @Fubbrr (Early development phase)
  - @ChenYX510 (Backend development)
  - @wukdhxbsdkjxn (Integration support)
  - @CoArrayLiu (Recent enhancements)

- **Commit Pattern**: Regular commits showing iterative development and bug fixes
- **Code Review**: Team collaboration evident from review and refinement commits

## 🎯 Key Features

### Frontend Capabilities
- Modern Vue 3 composition API
- Responsive UI with Element Plus components
- Client-side routing with Vue Router
- State management with Vuex
- Chinese language support with pinyin conversion
- RESTful API integration via Axios

### Backend Capabilities
- RESTful API endpoints
- Scalable ASP.NET Core architecture
- Potential database integration
- Service-oriented design

## 📊 Project Statistics
- **Repository Size**: ~667 KB
- **Network**: 3 forks
- **Open Issues**: 0
- **Watchers**: 1
- **Last Updated**: December 3, 2025

## 🚀 Getting Started for New Contributors

1. **Fork the repository** on GitHub
2. **Clone** your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/MelodyMuse.git
   cd MelodyMuse
   ```

3. **Install dependencies**:
   - Frontend: `cd melodymuse.client && npm install`
   - Backend: Open `MelodyMuse.sln` in Visual Studio

4. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

5. **Make your changes** and commit with clear messages

6. **Push to your fork** and create a Pull Request

## 📝 Development Standards

### Code Style
- JavaScript/Vue: Enforce with ESLint
- Follow project linting rules: `npm run lint`
- Use Vue 3 Composition API where possible
- Maintain consistent C# conventions for backend

### Commit Guidelines
- Use clear, descriptive commit messages
- Reference issues when applicable
- Keep commits focused and atomic
- Regular commits prevent large integration headaches

## 🔗 Useful Links

- **GitHub Repository**: [saikewei/MelodyMuse](https://github.com/saikewei/MelodyMuse)
- **Commit History**: [View all commits](https://github.com/saikewei/MelodyMuse/commits/master)
- **Issues**: [Project Issues](https://github.com/saikewei/MelodyMuse/issues)

## 📄 License

This project is currently open source. Check repository for specific license details.

## 👥 Contributors

We appreciate all contributions! Project developed and maintained by:
- [@saikewei](https://github.com/saikewei) - Project Lead
- [@Fubbrr](https://github.com/Fubbrr) - Frontend Development
- [@ChenYX510](https://github.com/ChenYX510) - Backend Development
- [@CoArrayLiu](https://github.com/CoArrayLiu) - Enhancement & Optimization
- [@wukdhxbsdkjxn](https://github.com/wukdhxbsdkjxn) - Integration Support

## 📧 Contact & Support

For questions, suggestions, or issues:
- Open an [Issue](https://github.com/saikewei/MelodyMuse/issues)
- Check existing discussions in the repository

---

**Last Updated**: December 2025  
**Status**: Actively Maintained ✅
