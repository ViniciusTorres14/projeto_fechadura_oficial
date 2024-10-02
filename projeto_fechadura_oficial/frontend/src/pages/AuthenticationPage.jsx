import React, { useState } from 'react';
import api from '../api';

function AuthenticationPage() {
  const [authData, setAuthData] = useState({ username: '', password: '' });
  const [response, setResponse] = useState(null);
  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const endpoint = isLogin ? '/Autenticacao/login' : '/Autenticacao/register';
      const res = await api.post(endpoint, authData);
      setResponse(res.data);
      // Store the token in localStorage or a state management solution
      localStorage.setItem('token', res.data.token);
    } catch (error) {
      setError(error.response?.data?.message || 'An error occurred');
      setResponse(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>{isLogin ? 'Login' : 'Register'}</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="Username"
            value={authData.username}
            onChange={(e) => setAuthData({ ...authData, username: e.target.value })}
          />
        </div>
        <div className="mb-3">
          <input
            type="password"
            className="form-control"
            placeholder="Password"
            value={authData.password}
            onChange={(e) => setAuthData({ ...authData, password: e.target.value })}
          />
        </div>
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? (isLogin ? 'Logging in...' : 'Registering...') : (isLogin ? 'Login' : 'Register')}
        </button>
      </form>
      <button className="btn btn-link" onClick={() => setIsLogin(!isLogin)}>
        {isLogin ? 'Switch to Register' : 'Switch to Login'}
      </button>
      {error && <div className="alert alert-danger mt-3">{error}</div>}
      {response && response.token && (
        <div className="alert alert-success mt-3">
          Authentication successful! Token: {response.token.substring(0, 20)}...
        </div>
      )}
    </div>
  );
}

export default AuthenticationPage;