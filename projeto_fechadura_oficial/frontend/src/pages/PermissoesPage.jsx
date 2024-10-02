import React, { useState, useEffect } from 'react';
import api from '../api';   

function PermissoesPage() {
  const [permissoes, setPermissoes] = useState([]);
  const [newPermissao, setNewPermissao] = useState({ permissionKey: '', descricao: '' });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchPermissoes();
  }, []);

  const fetchPermissoes = async () => {
    setLoading(true);
    try {
      const res = await api.get('/Permissoes');
      setPermissoes(res.data.permissoes);
    } catch (error) {
      setError('Erro ao buscar Permissões.');
      console.error('Error fetching permissoes:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!newPermissao.permissionKey || !newPermissao.descricao) {
      setError('Por favor, forneça tanto a Chave de Permissão quanto a Descrição.');
      return;
    }

    try {
      await api.post('/Permissoes', newPermissao);
      fetchPermissoes();
      setNewPermissao({ permissionKey: '', descricao: '' });
      setError(null);
    } catch (error) {
      setError('Erro ao criar Permissão.');
      console.error('Error creating permissao:', error);
    }
  };

  return (
    <div>
      <h2>Permissões</h2>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-danger">{error}</div>}
        <div className="row mb-3">
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="Chave de Permissão"
              value={newPermissao.permissionKey}
              onChange={(e) => setNewPermissao({ ...newPermissao, permissionKey: e.target.value })}
              required
            />
          </div>
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="Descrição"
              value={newPermissao.descricao}
              onChange={(e) => setNewPermissao({ ...newPermissao, descricao: e.target.value })}
              required
            />
          </div>
          <div className="col-auto">
            <button type="submit" className="btn btn-primary">Adicionar Permissão</button>
          </div>
        </div>
      </form>

      {loading ? (
        <div>Carregando...</div>
      ) : permissoes.length > 0 ? (
        <table className="table table-striped mt-4">
          <thead>
            <tr>
              <th>ID</th>
              <th>Chave de Permissão</th>
              <th>Descrição</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {permissoes.map((permissao) => (
              <tr key={permissao.permissaoId}>
                <td>{permissao.permissaoId}</td>
                <td>{permissao.permissionKey}</td>
                <td>{permissao.descricao}</td>
                <td>
                  <button className="btn btn-sm btn-secondary me-2">Editar</button>
                  <button className="btn btn-sm btn-danger">Excluir</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <div>Nenhuma Permissão encontrada.</div>
      )}
    </div>
  );
}

export default PermissoesPage;