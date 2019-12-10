import React from 'react';
import './LoadImage.css';

const _loaded = {};

export class LoadImage extends React.Component {


    // Initial state: image loaded stage 
    state = {
        loaded: _loaded[this.props.src]
    };

    // Define our loading and loaded image classes
    static defaultProps = {
        className: "",
        loadingClassName: "img-loading",
        loadedClassName: "img-loaded"
    };

    // Image onLoad handler to update state to loaded
    onLoad = () => {
        _loaded[this.props.src] = true;
        this.setState(() => ({ loaded: true }));
    };

    // Renderer
    render() {

        let { className, loadedClassName, loadingClassName } = this.props;

        className = `${className} ${this.state.loaded
            ? loadedClassName
            : loadingClassName}`;

        return <img
            id={this.props.id}
            src={this.props.src}
            onClick={this.props.onClick}
            className={className}
            onLoad={this.onLoad}
            alt={this.props.alt} />;
    }
}