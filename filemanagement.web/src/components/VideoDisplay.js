import React from "react";
import ReactPlayer from "react-player";

const VideoDisplay = (props) => {
  return (
    <div className="outer">
      <div className="inner">
        <ReactPlayer url={props.file} controls />
      </div>
    </div>
  );
};

export default VideoDisplay;
