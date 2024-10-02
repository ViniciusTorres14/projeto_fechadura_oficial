import React from 'react';
import { Link } from 'react-router-dom';
import { Navbar as BootstrapNavbar, Nav } from 'react-bootstrap';


function Navbar() {
  return (
    <BootstrapNavbar bg="dark" variant="dark" expand="lg">
      <div className="container">
        <BootstrapNavbar.Brand as={Link} to="/">
          6D Controle de acesso
        </BootstrapNavbar.Brand>
        <BootstrapNavbar.Toggle aria-controls="basic-navbar-nav" />
        <BootstrapNavbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto" >
            <Nav.Link as={Link} to="/access">Acesso salas de aula</Nav.Link>
            <Nav.Link as={Link} to="/authentication">Login</Nav.Link>
            {/* <Nav.Link as={Link} to="/cargo-permissoes">Cargo Permissões</Nav.Link> */}
            {/* <Nav.Link as={Link} to="/permissoes">Permissões</Nav.Link> */}
            <Nav.Link as={Link} to="/usuarios">Usuários</Nav.Link>
            {/* <Nav.Link as={Link} to="/cargos">Cargos</Nav.Link> */}
            <Nav.Link as={Link} to="/registros-de-acesso">Registros de Acesso</Nav.Link>
            <Nav.Link as={Link} to="/salas"> Gerenciar Salas</Nav.Link>
          </Nav>
        </BootstrapNavbar.Collapse>
      </div>
    </BootstrapNavbar>
  );
}

export default Navbar;