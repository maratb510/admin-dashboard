import { useEffect, useState } from 'react';
import axios from 'axios';

type Client = {
  id: number;
  name: string;
  email: string;
  balanceT: number;
};

type Rate = {
  value: number;
};

const DashboardPage = () => {
  const [clients, setClients] = useState<Client[]>([]);
  const [rate, setRate] = useState<Rate | null>(null);
  const [newRate, setNewRate] = useState('');
  const [message, setMessage] = useState('');

  const token = localStorage.getItem('token');

  const api = axios.create({
    baseURL: 'https://localhost:5000/api',
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  useEffect(() => {
    api.get('/clients').then(res => setClients(res.data));
    api.get('/rate').then(res => setRate(res.data));
  }, []);

  const handleUpdateRate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post('/rate', { value: parseFloat(newRate) });
      setMessage('Курс обновлён');
      const res = await api.get('/rate');
      setRate(res.data);
      setNewRate('');
    } catch (err) {
      setMessage('Ошибка при обновлении курса');
    }
  };

  return (
    <div style={{ padding: 30 }}>
      <h2>Клиенты</h2>
      <table border={1} cellPadding={8} style={{ marginBottom: 30 }}>
        <thead>
          <tr>
            <th>Имя</th>
            <th>Email</th>
            <th>Баланс (T)</th>
          </tr>
        </thead>
        <tbody>
          {clients.map(c => (
            <tr key={c.id}>
              <td>{c.name}</td>
              <td>{c.email}</td>
              <td>{c.balanceT}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h3>Курс токена</h3>
      <p>Текущий курс: <strong>{rate?.value}</strong></p>

      <form onSubmit={handleUpdateRate} style={{ marginTop: 10 }}>
        <input
          type="number"
          step="0.01"
          placeholder="Новый курс"
          value={newRate}
          onChange={e => setNewRate(e.target.value)}
        />
        <button type="submit" style={{ marginLeft: 10 }}>Обновить</button>
      </form>

      {message && <p style={{ marginTop: 10 }}>{message}</p>}
    </div>
  );
};

export default DashboardPage;
