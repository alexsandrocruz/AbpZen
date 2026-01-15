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
  import { useCreateAiChatSession, useUpdateAiChatSession } from "@/lib/abp/hooks/useAiChatSessions";
import { toast } from "sonner";

const formSchema = z.object({
  
    title: z.any(),
  
    contextType: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AiChatSessionFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AiChatSessionForm({
  isOpen,
  onClose,
  initialValues,
}: AiChatSessionFormProps) {
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

  const createMutation = useCreateAiChatSession();
const updateMutation = useUpdateAiChatSession();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("AiChatSession updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("AiChatSession created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save aichatsession:", error);
    toast.error(error.message || "Failed to save aichatsession");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit AiChatSession": "Create AiChatSession" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the aichatsession." : "Fill in the details to create a new aichatsession." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="title" > Title * </Label>

<Input id="title" {...register("title") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="contextType" > ContextType</Label>

<Input id="contextType" {...register("contextType") } />

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
