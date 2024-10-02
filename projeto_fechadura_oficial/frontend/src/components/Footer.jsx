import React from 'react';

function Footer() {
  return (
    <footer className="bg-dark text-white text-center py-3 mt-4">
      © {new Date().getFullYear()} 6D Projetos. Todos os direitos reservados.
    </footer>
  );
}

export default Footer;