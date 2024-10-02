import React, { useState } from 'react';
import api from '../api';

function AccessPage() {
  const [accessRequest, setAccessRequest] = useState({ RFID: '', PinCode: '', RoomId: '' });
  const [response, setResponse] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const res = await api.post('/Access/request', accessRequest);
      setResponse(res.data);
    } catch (error) {
      setError(error.response?.data?.message || 'An error occurred');
      setResponse(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Request Access</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="RFID"
            value={accessRequest.RFID}
            onChange={(e) => setAccessRequest({ ...accessRequest, RFID: e.target.value })}
          />
        </div>
        <div className="mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="Pin Code"
            value={accessRequest.PinCode}
            onChange={(e) => setAccessRequest({ ...accessRequest, PinCode: e.target.value })}
          />
        </div>
        <div className="mb-3">
          <input
            type="number"
            className="form-control"
            placeholder="Room ID"
            value={accessRequest.RoomId}
            onChange={(e) => setAccessRequest({ ...accessRequest, RoomId: e.target.value })}
          />
        </div>
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Requesting...' : 'Request Access'}
        </button>
      </form>
      {error && <div className="alert alert-danger mt-3">{error}</div>}
      {response && (
        <div className={`alert ${response.message.includes('concedido') ? 'alert-success' : 'alert-warning'} mt-3`}>
          {response.message}
        </div>
      )}
    </div>
  );
}

export default AccessPage;