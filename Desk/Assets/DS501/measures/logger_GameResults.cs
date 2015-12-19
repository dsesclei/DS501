using UnityEngine;
using System.Collections;
using System.IO;

public class logger_GameResults {
    // This writes info on game results, 
    //  in a format we can feed into R

    public string filename;

    public    int participant_id = -1;
    public string input_name = "NO INPUT SET";
    //public string interface_name;
    //public string task_name;

    public logger_GameResults()
    {

        System.IO.Directory.CreateDirectory("data/");

        double now = misc.get_timestamp();
        this.filename = "data/GameResults_" + now;
        this.filename = filename + ".csv";

        Debug.Log("Building logger: " + filename);

        // write header
        CSV.write(this.filename,
                    "timestamp", "participant",
                    "input",
                    "task_number", "task",
                    "result",
                    "elapsed"
                  );
    }

    public void log(       int task_number,
                        string task_name,
                        string results,
                        string elapsed
                    )
    {
        // get time elapsed since returned to start
        double now = misc.get_timestamp();
        
        // write the acutal log
        CSV.write(filename,
                    now, participant_id,
                    input_name,
                    task_number, task_name,
                    results,
                    elapsed
                  );
    }

}
