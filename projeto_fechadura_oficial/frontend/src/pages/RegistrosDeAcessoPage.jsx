import React, { useState, useEffect } from 'react';
import api from '../api';

function RegistrosDeAcessoPage() {
  const [registros, setRegistros] = useState([]);

  useEffect(() => {
    fetchRegistros();
  }, []);

  const fetchRegistros = async () => {
    try {
      const res = await api.get('/RegistrosDeAcesso');
      setRegistros(res.data.registrosDeAcesso);
    } catch (error) {
      console.error('Error fetching registros de acesso:', error);
    }
  };

  return (
    <div>
      <h2>Registros de Acesso</h2>
      <table className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Usuário ID</th>
            <th>Sala ID</th>
            <th>Access Time</th>
            <th>Access Granted</th>
          </tr>
        </thead>
        <tbody>
          {registros.map((r) => (
            <tr key={r.registroId}>
              <td>{r.registroId}</td>
              <td>{r.usuarioId}</td>
              <td>{r.salaId}</td>
              <td>{new Date(r.accessTime).toLocaleString()}</td>
              <td>{r.accessGranted ? 'Yes' : 'No'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default RegistrosDeAcessoPage;