# Polygon-Manipulator
- To add new polygon click: 'Add new polygon' button
- To clear canvas click: 'Clear Canvas' button
- To use Bresenham algorithm to draw lines instead of library algorithm tick 'Use Bresenham algorithm'
- To generate example scene click: 'Scene 1' button
- To add new point to current polygon click middle mouse button on canvas
- Polygon's points MUST create a cycle, otherwise no opperation are available besides adding new points to currently 
- selected polygon.
- To close cycle middle click on first point of polygon  
- To move point/line/polygon left click on it and move mouse in new place (selection priority point>line>polygon)
- Currently selcted point/line/polygon are highlighted
- To delele point/line right click on it and select 'Delete point'/'Delete line' for context menu
- To add new point in the middle of a line right click on the line and select 'Add point in the middle' from context menu
- To make two lines parallel:
	- 1 Right click on the first line and select 'Add constraint parallel'
	- 2 Now, the next left click will try to add constraint on another line, if you click other mouse button or 
	   somewhere where there is no line, adding constraint will be cancelled, nothing will change
- To impose certain length on line:
	- 1 Right click on the line and select 'Add constraint on length'
	- 2 Dialog box will appear asking you to enter new length for selected line
- To delete all constraints associated with a line rigth click on the line and select 'Delete constraints', it will
remove all constraints associated with the line
- Delete point/line or adding point in the middle of the line will remove all constraitns from adjecent points and lines
