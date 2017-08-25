for KILLPID in `ps ax | grep 'python' | awk ' { print $1;}'`; do 
  kill -9 $KILLPID;
done


echo "Python processes killed"

for KILLPID in `ps ax | grep 'java' | awk ' { print $1;}'`; do 
  kill -9 $KILLPID;
done
echo "Java processes killed"

echo "Starting python"
nohup python3 db_server.py>log.txt & nohup python3 lobby_server.py>log.txt & nohup python3 leela_server1.py>log.txt & nohup python3 game_server.py>log.txt & nohup python3 chat_server.py>log.txt & 
sleep 4
echo "Starting java"
nohup java -jar ../slave.jar>log.txt
echo "Restarted"
