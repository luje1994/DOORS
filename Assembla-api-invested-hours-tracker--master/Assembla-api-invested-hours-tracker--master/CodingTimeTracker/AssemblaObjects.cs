using System;
using System.Runtime.Serialization;

public class Ticket
{
    public string Id { get; set; }

    public int number { get; set; }

    public string summary { get; set; }

    public string description { get; set; }

    public byte priority { get; set; }

    public DateTime? completed_date { get; set; }

    public string component_id { get; set; }

    public DateTime? created_on { get; set; }

    public int permission_type { get; set; }

    public float importance { get; set; }

    public bool is_story { get; set; }

    public int milestone_id { get; set; }

    public string notification_list { get; set; }

    public string space_id { get; set; }

    public int state { get; set; }

    public string status { get; set; }

    public int story_importance { get; set; }

    public DateTime? updated_at { get; set; }

    public float working_hours { get; set; }

    public float estimate { get; set; }

    public float total_estimate { get; set; }

    public float total_invested_hours { get; set; }

    public float total_working_hours { get; set; }

    public string assigned_to_id { get; set; }

    public string reporter_id { get; set; }

    //public string custom_fields { get; set; }

    public int hierarchy_type { get; set; }
}

public class TicketComment
{
    public int Id { get; set; }

    public string comment { get; set; }

    public int ticket_id { get; set; }

    public string user_id { get; set; }

    public DateTime? created_on { get; set; }

    public DateTime? updated_at { get; set; }

    public string ticket_changes { get; set; }
}

[DataContract]
public class UserTask
{
    [DataMember(Name = "user_task")]
    public Task Tasks { get; set; }
} 

public class Task
{
    [DataMember]
    public int id { get; set; }

    [DataMember]
    public string description { get; set; }

    [DataMember]
    public bool billed { get; set; }

    [DataMember]
    public string user_id { get; set; }

    [DataMember]
    public string job_agreement_id { get; set; }

    [DataMember]
    public string space_id { get; set; }

    [DataMember]
    public int ticket_id { get; set; }

    [DataMember]
    public string url { get; set; }

    [DataMember]
    public float hours { get; set; }

    [DataMember]
    public DateTime begin_at { get; set; }

    [DataMember]
    public DateTime end_at { get; set; }

    [DataMember]
    public DateTime? created_at { get; set; }

    [DataMember]
    public DateTime? updated_at { get; set; }
}

public class Space
{
    public string id { get; set; }

    public string name { get; set; }

    public string description { get; set; }

    public string wiki_name { get; set; }

    public int public_permissions { get; set; }

    public int team_permissions { get; set; }

    public int watcher_permissions { get; set; }

    public bool share_permissions { get; set; }

    public int team_tab_role { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    public string default_showpage { get; set; }

    //this may throw error 
    public string tabs_order { get; set; }

    public string parent_id { get; set; }

    public bool restricted { get; set; }

    public DateTime? restricted_date { get; set; }

    public DateTime commercial_from { get; set; }

    public string banner { get; set; }

    public string banner_height { get; set; }

    public string banner_text { get; set; }

    public string banner_link { get; set; }

    public string style { get; set; }

    public int status { get; set; }

    public bool approved { get; set; }

    public bool is_manager { get; set; }

    public bool is_volunteer { get; set; }

    public bool is_commercial { get; set; }

    public bool can_join { get; set; }

    public DateTime? last_payer_changed_at { get; set; }
}

public class User
{
    public string id { get; set; }

    public string login { get; set; }

    public string name { get; set; }

    public string email { get; set; }

    public string organization { get; set; }

    public string phone { get; set; }

    public string picture { get; set; }
}

