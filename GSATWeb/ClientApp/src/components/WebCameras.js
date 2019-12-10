import React, { Component } from 'react';
import { LoadImage } from './LoadImage.js';

export class WebCameras extends Component {
    static displayName = WebCameras.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }

    componentDidMount() {
        this.populateRandomCamera();
    }

    render() {

        return (
            <div>
                <p>
                    <h3>Random Broward County Traffic Camera</h3>
                    <LoadImage id="imgRand" alt="Random Traffic Camera in Broward County Florida"></LoadImage>
                </p>
                <p>
                    <h3>Interstate 95 South of Oakland Park Blvd Traffic Camera</h3>
                    <LoadImage src='WebCams/1' alt="I-95 South of Oakland in Broward County Florida"></LoadImage>
                </p>
            </div>
        );
    }

    async populateRandomCamera() {

        // Pick a random camera number between 2 and 7
        let randycam = Math.floor((Math.random() * 5) + 2);

        // Get the random traffic cam image
        fetch('WebCams/' + randycam)
            .then(rez => rez.blob())
            .then(blob => {
                var outside = URL.createObjectURL(blob)
                // document.getElementsByTagName("img")[0].src = outside;
                var dirk = document.getElementById('imgRand');
                dirk.src = outside;
            })
    }
}
