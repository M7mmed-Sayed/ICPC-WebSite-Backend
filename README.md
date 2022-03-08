
# Competitive Programining Information System

## System description

- The system has multible roles.
  - Administrator  ,_may be more than one_ his role is to mange the whole site
  - Community leader, his role is to mange everything in the community.
  - Training Head ,_may be more than one_ his role is to manage all trainings in the community.
  - Training manager ,_may be more than one_ his role is to manage specifc training only.(**Newcomers Training Term 1** as an example)
  - Mentor ,_More than one_ his role is the ability to view the analysis of some trainees assigned to him.
  - Trainee ,_More than one_ he can view his analysis.

- Website structure
  - **Home**: quick overview of the services that are provided.
  - **Posts**: posts, blogs or news that are related to the site (not for specifc community).
  - **Communities**:Display all the communites that are registerd on the website
  - **Contests**: public contests for all communities.
  - **Editor**: users can complie any code and see the output _like codeforces custom.
  - **About**:Information about the website in details and adminstrators information.
  - **Contact Us**:Form to send an email to the adminstrators.

- Community structure
  - **Home**: quick overview of the services that are provided.
  - **Posts**: posts, blogs or news that are related to the site (not for specifc community).
  - **Contests**: contest for this community only(public and private).
  - **Trainings**:Display all the communites that are registerd on the website
  - **Editor**: users can complie any code and see the output _like codeforces custom.
  - **About**:Information about the website in details and adminstrators information.
  - **Contact Us**:Form to send an email to the adminstrators.
- System Requirments
  - guest users can see public posts ,communities and can use the online editor.
  - users can request to create a new community, then the adminestrator will recive this request and can accept or refuse. in case of acceptence the requester will be the community leader
  - users Can request to join any existing community and the community leader accept or refuse this requset
  - the user can apply in any training in the  communities he is accepted to join it , and the training manager can accept this request or refuse it based on the training critria, in case of acceptence the user can see his analsys in this training and will be assigend to a mentor,otherwise he can practise individualy from the communty roadmap and can see his analsys in his profile
  - every training has analysis, standing and material.
  - in the analysis trainees can see the progress of every trainees in this training (how many problem solved every day)
  - in the standing there is a list of trainees in the training sorted by the number of solved problems or by his points and the training maneger will define the number of trainees to display.
  - in the material
    - there are the weeks that contains tutorial for the currrent week and a sheet of the problems.
    - there will be a contests also after each week.