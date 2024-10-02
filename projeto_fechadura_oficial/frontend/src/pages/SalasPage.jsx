import React, { useState, useEffect } from 'react';
import api from '../api';

function SalasPage() {
  const [salas, setSalas] = useState([]);
  const [newSala, setNewSala] = useState({ nome: '', descricao: '', status: false });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchSalas();
  }, []);

  const fetchSalas = async () => {
    setLoading(true);
    try {
      const res = await api.get('/Salas');
      setSalas(res.data.salas);
    } catch (error) {
      setError('Erro ao buscar Salas.');
      console.error('Error fetching salas:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!newSala.nome || !newSala.descricao) {
      setError('Por favor, forneça tanto o Nome quanto a Descrição.');
      return;
    }

    try {
      await api.post('/Salas', newSala);
      fetchSalas();
      setNewSala({ nome: '', descricao: '', status: false });
      setError(null);
    } catch (error) {
      setError('Erro ao criar Sala.');
      console.error('Error creating sala:', error);
    }
  };

  return (
    <div>
      <h2>Salas</h2>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-danger">{error}</div>}
        <div className="mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="Nome"
            value={newSala.nome}
            onChange={(e) => setNewSala({ ...newSala, nome: e.target.value })}
            required
          />
        </div>
        <div className="mb-3">
          <textarea
            className="form-control"
            placeholder="Descrição"
            value={newSala.descricao}
            onChange={(e) => setNewSala({ ...newSala, descricao: e.target.value })}
            required
          />
        </div>
        <div className="mb-3 form-check">
          <input
            type="checkbox"
            className="form-check-input"
            id="statusCheck"
            checked={newSala.status}
            onChange={(e) => setNewSala({ ...newSala, status: e.target.checked })}
          />
          <label className="form-check-label" htmlFor="statusCheck">Disponível</label>
        </div>
        <button type="submit" className="btn btn-primary">Adicionar Sala</button>
      </form>

      {loading ? (
        <div>Carregando...</div>
      ) : salas.length > 0 ? (
        <table className="table table-striped mt-4">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>Descrição</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {salas.map((sala) => (
              <tr key={sala.salaId}>
                <td>{sala.salaId}</td>
                <td>{sala.nome}</td>
                <td>{sala.descricao}</td>
                <td>{sala.status ? 'Disponível' : 'Ocupada'}</td>
                <td>
                  <button className="btn btn-sm btn-secondary me-2">Editar</button>
                  <button className="btn btn-sm btn-danger">Excluir</button>
                  <button className="btn btn-sm btn-secondary me-2">Reservar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <div>Nenhuma Sala encontrada.</div>
      )}
    </div>
  );
}

export default SalasPage;