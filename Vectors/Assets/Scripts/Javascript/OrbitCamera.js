var target : Transform;
private var edgeBorder : float = 0.1;
private var horizontalSpeed : float = 360.0;
private var verticalSpeed : float = 120.0;
private var minHorizontal : float = -125.0;
private var maxHorizontal : float = 35.0;
private var minVertical : float = 0.0;
private var maxVertical : float = 90.0;
private var x : float = 0.0;
private var y : float = 0.0;
private var distance : float = 0.0;

function Start() {
	x = transform.eulerAngles.y;
	y = transform.eulerAngles.x;
	distance = (transform.position - target.position).magnitude;
}

function LateUpdate() {
	var dt = Time.deltaTime;
	x -= Input.GetAxis("Horizontal") * horizontalSpeed * dt;
	y += Input.GetAxis("Vertical") * verticalSpeed * dt;
	
	x = ClampAngle(x, minHorizontal, maxHorizontal);
	y = ClampAngle(y, minVertical, maxVertical);
	
	var orientation = Quaternion.Euler(y, x, 0);
	var location = orientation * Vector3(0.0, 0.0, -distance) + target.position;
	
	transform.rotation = orientation;
	transform.position = location;
}

static function ClampAngle(angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp(angle, min, max);
}