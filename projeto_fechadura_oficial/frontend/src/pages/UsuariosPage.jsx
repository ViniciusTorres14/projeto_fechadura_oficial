import React, { useState, useEffect } from 'react';
import api from '../api';

function UsuariosPage() {
  const [usuarios, setUsuarios] = useState([]);
  const [newUsuario, setNewUsuario] = useState({ nome: '', email: '', password: '' });

  useEffect(() => {
    fetchUsuarios();
  }, []);

  const fetchUsuarios = async () => {
    try {
      const res = await api.get('/Usuarios');
      setUsuarios(res.data.usuarios);
    } catch (error) {
      console.error('Error fetching usuarios:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post('/Usuarios', newUsuario);
      fetchUsuarios();
      setNewUsuario({ nome: '', email: '', password: '' });
    } catch (error) {
      console.error('Error creating usuario:', error);
    }
  };

  return (
    <div>
      <h2>Criar usuário</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="Nome"
            value={newUsuario.nome}
            onChange={(e) => setNewUsuario({ ...newUsuario, nome: e.target.value })}
          />
        </div>
        <div className="mb-3">
          <input
            type="email"
            className="form-control"
            placeholder="Email"
            value={newUsuario.email}
            onChange={(e) => setNewUsuario({ ...newUsuario, email: e.target.value })}
          />
        </div>
        <div className="mb-3">
          <input
            type="password"
            className="form-control"
            placeholder="Password"
            value={newUsuario.password}
            onChange={(e) => setNewUsuario({ ...newUsuario, password: e.target.value })}
          />
        </div>
        <button type="submit" className="btn btn-primary">Adicionar Usuário</button>
      </form>
      <h3 className="mt-4">Usuários existentes</h3>
      <ul className="list-group">
        {usuarios.map((u) => (
          <li key={u.usuarioId} className="list-group-item">
            {u.nome} ({u.email})
          </li>
        ))}
      </ul>
    </div>
  );
}

export default UsuariosPage;