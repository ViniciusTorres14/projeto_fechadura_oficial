import React, { useState, useEffect } from 'react';
import api from '../api';

function CargoPermissoesPage() {
  const [cargoPermissoes, setCargoPermissoes] = useState([]);
  const [newCargoPermissao, setNewCargoPermissao] = useState({ cargoId: '', permissaoId: '' });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCargoPermissoes();
  }, []);

  const fetchCargoPermissoes = async () => {
    setLoading(true);
    try {
      const res = await api.get('/CargoPermissoes');
      setCargoPermissoes(res.data.cargoPermissoes);
    } catch (error) {
      setError('Error fetching cargo permissoes');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await api.post('/CargoPermissoes', newCargoPermissao);
      fetchCargoPermissoes();
      setNewCargoPermissao({ cargoId: '', permissaoId: '' });
    } catch (error) {
      setError('Error creating cargo permissao');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Cargo Permissões</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <input
            type="number"
            className="form-control"
            placeholder="Cargo ID"
            value={newCargoPermissao.cargoId}
            onChange={(e) => setNewCargoPermissao({ ...newCargoPermissao, cargoId: e.target.value })}
          />
        </div>
        <div className="mb-3">
          <input
            type="number"
            className="form-control"
            placeholder="Permissão ID"
            value={newCargoPermissao.permissaoId}
            onChange={(e) => setNewCargoPermissao({ ...newCargoPermissao, permissaoId: e.target.value })}
          />
        </div>
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Adding...' : 'Add Cargo Permissão'}
        </button>
      </form>
      {error && <div className="alert alert-danger mt-3">{error}</div>}
      {loading ? (
        <div>Loading...</div>
      ) : (
        <ul className="list-group mt-3">
          {cargoPermissoes.map((cp) => (
            <li key={cp.cargoPermissaoId} className="list-group-item">
              Cargo ID: {cp.cargoId}, Permissão ID: {cp.permissaoId}
              <button className="btn btn-sm btn-danger float-end" onClick={() => handleDelete(cp.cargoPermissaoId)}>Delete</button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default CargoPermissoesPage;