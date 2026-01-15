import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateTimeEntry, useUpdateTimeEntry } from "@/lib/abp/hooks/useTimeEntries";
import { toast } from "sonner";

const formSchema = z.object({
  
    description: z.any(),
  
    hours: z.any(),
  
    date: z.any(),
  
    projectId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface TimeEntryFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function TimeEntryForm({
  isOpen,
  onClose,
  initialValues,
}: TimeEntryFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateTimeEntry();
const updateMutation = useUpdateTimeEntry();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("TimeEntry updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("TimeEntry created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save timeentry:", error);
    toast.error(error.message || "Failed to save timeentry");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit TimeEntry": "Create TimeEntry" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the timeentry." : "Fill in the details to create a new timeentry." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="description" > Description * </Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="hours" > Hours * </Label>

<Input id="hours" type = "number" step = "any" {...register("hours") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="date" > Date * </Label>

<Input id="date" type = "date" {...register("date") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="projectId" > ProjectId</Label>

<Input id="projectId" {...register("projectId") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
