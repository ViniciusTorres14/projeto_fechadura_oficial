import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import AccessPage from './pages/AccessPage';
import AuthenticationPage from './pages/AuthenticationPage';
import CargoPermissoesPage from './pages/CargoPermissoesPage';
import PermissoesPage from './pages/PermissoesPage';
import UsuariosPage from './pages/UsuariosPage';
import CargosPage from './pages/CargosPage';
import RegistrosDeAcessoPage from './pages/RegistrosDeAcessoPage';
import SalasPage from './pages/SalasPage';
function App() {
  return (
    <Router>
      <Navbar />
      <div className="container mt-4">
        <Routes>
          <Route path="/access" element={<AccessPage />} />
          <Route path="/authentication" element={<AuthenticationPage />} />
          <Route path="/cargo-permissoes" element={<CargoPermissoesPage />} />
          <Route path="/permissoes" element={<PermissoesPage />} />
          <Route path="/usuarios" element={<UsuariosPage />} />
          <Route path="/cargos" element={<CargosPage />} />
          <Route path="/registros-de-acesso" element={<RegistrosDeAcessoPage />} />
          <Route path="/salas" element={<SalasPage />} />
          <Route path="*" element={<AuthenticationPage />} /> {/* Default Route */}
        </Routes>
      </div>
      <Footer />
    </Router>
  );
}

export default App;