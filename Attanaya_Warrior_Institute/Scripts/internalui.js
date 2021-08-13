var Socket1 = new WebSocket('ws://192.168.1.3:81/');
var Socket2 = new WebSocket('ws://192.168.1.4:81/');
var SocketC = new WebSocket('ws://192.168.1.2:81/');

// Send room number and number of hours, set a timer in the room. 
function SetRoomToTime(room, time) {
    // SIM 01
    if (room === 2) {

        if (time === 1) {
            setOneHourSim1();
        }

        if (time === 2) {
            SetTwoHoursSim1();
        }

        if (time === 3) {
            SetThreehoursSim1();
        }

        if (time === 4) {
            SetFourHoursSim1();
        }

    }
    // SIM 02
    if (room === 3) {

        if (time === 1) {
            setOneHourSim2();
        }
        
        if (time === 2) {
            SetTwoHoursSim2();
        }

        if (time === 3) {
            SetThreehoursSim2();
        }

        if (time === 4) {
            SetFourHoursSim2();
        }

    }
    // SIM 03
    if (room === 4) {

        if (time === 1) {
            setOneHourSimC();
        }

        if (time ===2) {
            SetTwoHoursSimC();
        }

        if (time === 3) {
            SetThreehoursSimC();
        }

        if (time === 4) {
            SetFourHoursSimC();
        }

    }

}

// API CXalls to timers
function SendToSim1(nums) {
    Socket1.send("#" + nums);
}

function SendToSim2(nums) {
    Socket2.send("#" + nums);
}

function SendToSimC(nums) {
    SocketC.send("#" + nums);
}


// API call scripts for timers.
function setOneHourSim1() {
    SendToSim1(22695);
    SendToSim1(23205);
    SendToSim1(26775);
    SendToSim1(26775);
    SendToSim1(26775);
    SendToSim1(39015);
}

function setOneHourSim2() {
    SendToSim2(22695);
    SendToSim2(23205);
    SendToSim2(26775);
    SendToSim2(26775);
    SendToSim2(26775);
    SendToSim2(39015);
}

function setOneHourSimC() {
    SendToSimC(22695);
    SendToSimC(23205);
    SendToSimC(26775);
    SendToSimC(26775);
    SendToSimC(26775);
    SendToSimC(39015);
}

function SetTwoHoursSim1() {
    SendToSim1(59415);
    SendToSim1(26775);
    SendToSim1(6375);
    SendToSim1(26775);
    SendToSim1(26775);
    SendToSim1(39015);
}

function SetTwoHoursSim2() {
    SendToSim2(59415);
    SendToSim2(26775);
    SendToSim2(6375);
    SendToSim2(26775);
    SendToSim2(26775);
    SendToSim2(39015);
}

function SetTwoHoursSimC() {
    SendToSimC(59415);
    SendToSimC(26775);
    SendToSimC(6375);
    SendToSimC(26775);
    SendToSimC(26775);
    SendToSimC(39015);
}

function SetThreehoursSim1() {
    SendToSim1(59415);
    SendToSim1(26775);
    SendToSim1(31365);
    SendToSim1(26775);
    SendToSim1(26775);
    SendToSim1(39015);
}

function SetThreeHoursSim2() {
    SendToSim2(59415);
    SendToSim2(26775);
    SendToSim2(31365);
    SendToSim2(26775);
    SendToSim2(26775);
    SendToSim2(39015);
}

function SetThreeHoursSimC() {
    SendToSimC(59415);
    SendToSimC(26775);
    SendToSimC(31365);
    SendToSimC(26775);
    SendToSimC(26775);
    SendToSimC(39015);
}

function SetFourHoursSim1() {
    SendToSim1(59415);
    SendToSim1(26775);
    SendToSim1(4335);
    SendToSim1(26775);
    SendToSim1(26775);
    SendToSim1(39015);
}

function SetFourHoursSim2() {
    SendToSim2(59415);
    SendToSim2(26775);
    SendToSim2(4335);
    SendToSim2(26775);
    SendToSim2(26775);
    SendToSim2(39015);
}

function SetFourHoursSimC() {
    SendToSimC(59415);
    SendToSimC(26775);
    SendToSimC(4335);
    SendToSimC(26775);
    SendToSimC(26775);
    SendToSimC(39015);
}

