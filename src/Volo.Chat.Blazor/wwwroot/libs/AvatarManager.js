var VoloChatAvatarManager = {
	createCanvasForUser: function (canvas, username, name) {
	    if ((username == null || username == "") && (name == null || name == "")) {
            canvas.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);
            return;
        }

        var colors = [
            {text: '#ffffff', background: '#3cb160'},
            {text: '#ffffff', background: '#c373cc'},
            {text: '#ffffff', background: '#2b78b3'},
            {text: '#ffffff', background: '#6ac79a'},
            {text: '#ffffff', background: '#aeb140'},
            {text: '#ffffff', background: '#b773c0'},
            {text: '#ffffff', background: '#e16d7a'},
            {text: '#ffffff', background: '#ffac2a'},
            {text: '#ffffff', background: '#21bbc7'},
            {text: '#ffffff', background: '#59ab95'}
        ];

        var generateFromString = function (str) {
            var hash = 0;
            for (var i = 0; i < str.length; i++) {
                hash = str.charCodeAt(i) + ((hash << 5) - hash);
            }
            return colors[Math.abs(hash % 10)];
        }

		var hashText;
		var text;

		if (name && name.length > 0) {
			hashText = name;

			var nameSplited = name.trim().split(" ");

			if (nameSplited.length >= 2) {
				var firstName = nameSplited[0];
				var lastName = nameSplited[nameSplited.length - 1];

				text = firstName.length >= 1 ? firstName.substring(0, 1) : firstName;
				text += lastName.length >= 1 ? lastName.substring(0, 1) : lastName;
			} else {
				text = name.length >= 2 ? name.substring(0, 2) : name;
			}
		} else {
			hashText = username;
			text = username && username.length >= 2 ? username.substring(0, 2) : username;
		}

		var colors = generateFromString(hashText);

		var ctx = canvas.getContext("2d");

		ctx.fillStyle = colors.background;
		ctx.fillRect(0, 0, canvas.width, canvas.height);

		ctx.font = "bold 15px Arial";
		ctx.fillStyle = colors.text;
		ctx.fillText(text.toUpperCase().substring(0, 2), canvas.width / 2 - 10, canvas.height / 2 + 5);
	}
};
