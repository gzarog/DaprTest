import React, { useState } from 'react';
import axios from 'axios';

const OrderForm = () => {
    const [order, setOrder] = useState({
        Id: '',
        ItemName: '',
        Quantity: '',
        Source: '',
    });

    const handleChange = (e) => {
        setOrder({
            ...order,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('http://localhost:5000/api/orders', order);
            console.log(response.data);
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <div className="form-container">
            <form onSubmit={handleSubmit} className="order-form">
                <label>
                    Id:
                    <input type="number" name="Id" value={order.Id} onChange={handleChange} />
                </label>
                <label>
                    Item Name:
                    <input type="text" name="ItemName" value={order.ItemName} onChange={handleChange} />
                </label>
                <label>
                    Quantity:
                    <input type="number" name="Quantity" value={order.Quantity} onChange={handleChange} />
                </label>
                <label>
                    Source:
                    <input type="text" name="Source" value={order.Source} onChange={handleChange} />
                </label>
                <button type="submit">Submit Order</button>
            </form>
        </div>
    );
};




export default OrderForm;
