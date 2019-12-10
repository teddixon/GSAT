import React, { Component } from 'react';

export class PizzaTrend extends Component {
    static displayName = PizzaTrend.name;

    constructor(props) {
        super(props);
        this.state = { favorites: [], loading: true };
    }

    componentDidMount() {
        this.populateFavoritesData();
    }

    static renderFavoritesTable(favorites) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Count</th>
                        <th>Toppings</th>
                    </tr>
                </thead>
                <tbody>
                    {favorites.map(favorites =>
                        <tr key={favorites.count}>
                            <td>{favorites.count}</td>
                            <td>{favorites.toppings}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : PizzaTrend.renderFavoritesTable(this.state.favorites);

        return (
            <div>
                <h1 id="tabelLabel" >Current Trending Favorite Pizza Toppings</h1>
                <p>This component demonstrates fetching the most favorite Pizza topings from an order server.</p>
                {contents}
            </div>
        );
    }

    async populateFavoritesData() {
        const response = await fetch('pizza');
        const data = await response.json();
        this.setState({ favorites: data, loading: false });
    }
}
