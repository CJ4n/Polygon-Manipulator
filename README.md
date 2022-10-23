## Polygon-Manipulator
# Manual
- To add a new polygon click: 'Add new polygon' button
- To clear canvas click: 'Clear Canvas' button
- To use Bresenham algorithm to draw lines instead of library algorithm tick 'Use Bresenham algorithm'
- To generate an example scene click: 'Scene 1' button
- To add new point to current polygon, click middle button on mouse on canvas
- Polygon's points MUST create a cycle, otherwise no operations are available besides adding new points to currently 
  selected polygon.
- To close the cycle (the polygon), middle click on first point of polygon  
- To move point/line/polygon left click on it and move mouse in new place (selection priority point>line>polygon)
- Currently selected point/line/polygon are highlighted
- To delete point/line, right click on it and select 'Delete point'/'Delete line' from the context menu
- To add a new point in the middle of a line right click on the line and select 'Add point in the middle' from context menu
- To make two lines parallel:
	1. Right click on the first line and select 'Add constraint parallel'
	2. Now, the next left click will try to add constraint on another line, if you click other mouse button or 
	   someplace where there is no line, adding constraint will be cancelled, nothing will change
- To impose certain length on line:
	1. Right click on the line and select 'Add constraint on length'
	2. Dialog box will appear asking you to enter a new length for the selected line
- To delete all constraints associated with a line, right click on the line and select 'Delete constraints', it will
remove all constraints associated with the line
- Delete point/line or adding a point in the middle of the line will remove all constraints from adjacent points and lines  
# Some notes on implementation
- Polygon is represented as a circular linked list, and line is represented only by the 'first' vertex, since every vertex 
  has a reference to next (and previous) point
- No two adjacent lines can be parallel to each other
- When line is rotated, this is when there is parallel constraint, line is rotated around the point in the middle of 
  the line ((x1+x2)/2,(y1+y2)/2)
- When line has constraint on its length, then attempt to move one of its vertices will make second vertex follow it
- Each vertex has a list of constraints
- Constraints are executed recursively. It's kind of a DFS on a set of vertices, that can be reached from the first 
   vertex on which constraints are executed and visiting other 'ends of constraints' of this point and repeat this action

