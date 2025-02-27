bool down = false;

void setup() {
  Serial.begin(9600);
  pinMode(12, INPUT_PULLUP);
}

void loop() {
  if(digitalRead(12) == HIGH && down == false){
    Serial.println("1");
    down = true;
  }
  else if(digitalRead(12) == LOW && down == true){
    Serial.println("0");
    down = false;
  }
}
