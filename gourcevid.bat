gource ^
    -s 7.5 ^
    -f ^
    --title "Advanced Software Quality: Term Project - Isaiah, Devin, Dusan, Cole" ^
    -1920x1080 ^
    --auto-skip-seconds .01 ^
    --multi-sampling ^
    --stop-at-end ^
    --key ^
    --highlight-users ^
    --hide mouse,filenames ^
    --file-idle-time 0 ^
    --max-files 0  ^
    --background-colour 282828 ^
    --font-size 25 ^
    --font-colour EBDBB2 ^
    --output-ppm-stream - ^
    --output-framerate 30 ^
    | ffmpeg -y -r 30 -f image2pipe -vcodec ppm -i - -b 98304K Z:\videos\releaseCandidate2.mp4
