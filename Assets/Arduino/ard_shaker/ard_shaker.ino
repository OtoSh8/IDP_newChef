bool down = false;

void setup() {
  Serial.begin(9600);
  pinMode(12, INPUT_PULLUP);
  pinMode(11, OUTPUT);

}

void loop() {
  if(digitalRead(12) == HIGH && down == false){
    down = true;
  }
  else if(digitalRead(12) == LOW && down == true){
    Serial.println("0");
    down = false;
  }

  if(digitalRead(12) == HIGH){
      digitalWrite(11, LOW );

  }
  else{
  digitalWrite(11, HIGH);

  }

  delay(100);
}
