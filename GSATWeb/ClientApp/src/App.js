import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { WebCameras } from './components/WebCameras';
import { PizzaTrend } from './components/PizzaTrend';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/pizzatrend' component={PizzaTrend} />
                <Route path='/webcameras' component={WebCameras} />
            </Layout>
        );
    }
}
