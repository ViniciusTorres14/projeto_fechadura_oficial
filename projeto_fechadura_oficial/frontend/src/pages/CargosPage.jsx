import React, { useState, useEffect } from 'react';
import api from '../api';

function CargosPage() {
  const [cargos, setCargos] = useState([]);
  const [newCargo, setNewCargo] = useState({ roleName: '', descricao: '' });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCargos();
  }, []);

  const fetchCargos = async () => {
    setLoading(true);
    try {
      const res = await api.get('/Cargos');
      setCargos(res.data.cargos);
    } catch (error) {
      setError('Error fetching cargos');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await api.post('/Cargos', newCargo);
      fetchCargos();
      setNewCargo({ roleName: '', descricao: '' });
    } catch (error) {
      setError('Error creating cargo');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    try {
      await api.delete(`/Cargos/${id}`);
      fetchCargos();
    } catch (error) {
      setError('Error deleting cargo');
    }
  };

  return (
    <div>
      <h2>Cargos</h2>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-danger">{error}</div>}
        <div className="mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="Nome da Função"
            value={newCargo.roleName}
            onChange={(e) => setNewCargo({ ...newCargo, roleName: e.target.value })}
            required
          />
        </div>
        <div className="mb-3">
          <textarea
            className="form-control"
            placeholder="Descrição"
            value={newCargo.descricao}
            onChange={(e) => setNewCargo({ ...newCargo, descricao: e.target.value })}
            required
          />
        </div>
        <button type="submit" className="btn btn-primary">Adicionar Cargo</button>
      </form>

      {loading ? (
        <div>Carregando...</div>
      ) : cargos.length > 0 ? (
        <table className="table table-striped mt-4">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome da Função</th>
              <th>Descrição</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {cargos.map((cargo) => (
              <tr key={cargo.cargoId}>
                <td>{cargo.cargoId}</td>
                <td>{cargo.roleName}</td>
                <td>{cargo.descricao}</td>
                <td>
                  <button className="btn btn-sm btn-secondary me-2">Editar</button>
                  <button className="btn btn-sm btn-danger" onClick={() => handleDelete(cargo.cargoId)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <div>Nenhum Cargo encontrado.</div>
      )}
    </div>
  );
}

export default CargosPage;