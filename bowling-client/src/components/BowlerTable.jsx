import { useEffect, useState } from 'react';

export default function BowlerTable() {
  const [bowlers, setBowlers] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch('http://localhost:5286/api/bowlers')
      .then((res) => res.json())
      .then(setBowlers)
      .catch(() => setError('Failed to load bowler data.'));
  }, []);

  if (error) return <p>{error}</p>;
  if (bowlers.length === 0) return <p>Loading...</p>;

  return (
    <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>Team</th>
          <th>Address</th>
          <th>City</th>
          <th>State</th>
          <th>Zip</th>
          <th>Phone</th>
        </tr>
      </thead>
      <tbody>
        {bowlers.map((b, i) => (
          <tr key={i}>
            <td>{b.firstName} {b.middleInit ? b.middleInit + '. ' : ''}{b.lastName}</td>
            <td>{b.teamName}</td>
            <td>{b.address}</td>
            <td>{b.city}</td>
            <td>{b.state}</td>
            <td>{b.zip}</td>
            <td>{b.phone}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
