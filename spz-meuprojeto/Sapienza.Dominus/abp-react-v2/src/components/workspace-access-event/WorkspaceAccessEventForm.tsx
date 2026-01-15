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
  import { useCreateWorkspaceAccessEvent, useUpdateWorkspaceAccessEvent } from "@/lib/abp/hooks/useWorkspaceAccessEvents";
import { toast } from "sonner";

const formSchema = z.object({
  
    eventType: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface WorkspaceAccessEventFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function WorkspaceAccessEventForm({
  isOpen,
  onClose,
  initialValues,
}: WorkspaceAccessEventFormProps) {
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

  const createMutation = useCreateWorkspaceAccessEvent();
const updateMutation = useUpdateWorkspaceAccessEvent();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("WorkspaceAccessEvent updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("WorkspaceAccessEvent created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save workspaceaccessevent:", error);
    toast.error(error.message || "Failed to save workspaceaccessevent");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit WorkspaceAccessEvent": "Create WorkspaceAccessEvent" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the workspaceaccessevent." : "Fill in the details to create a new workspaceaccessevent." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="eventType" > EventType * </Label>

<Input id="eventType" {...register("eventType") } />

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
