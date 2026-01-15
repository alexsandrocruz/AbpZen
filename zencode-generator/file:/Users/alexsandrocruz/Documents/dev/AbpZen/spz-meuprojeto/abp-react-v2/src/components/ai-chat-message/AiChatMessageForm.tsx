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
  import { useCreateAiChatMessage, useUpdateAiChatMessage } from "@/lib/abp/hooks/useAiChatMessages";
import { toast } from "sonner";

const formSchema = z.object({
  
    role: z.any(),
  
    content: z.any(),
  
    sessionId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AiChatMessageFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AiChatMessageForm({
  isOpen,
  onClose,
  initialValues,
}: AiChatMessageFormProps) {
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

  const createMutation = useCreateAiChatMessage();
const updateMutation = useUpdateAiChatMessage();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("AiChatMessage updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("AiChatMessage created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save aichatmessage:", error);
    toast.error(error.message || "Failed to save aichatmessage");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit AiChatMessage": "Create AiChatMessage" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the aichatmessage." : "Fill in the details to create a new aichatmessage." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="role" > Role * </Label>

<Input id="role" {...register("role") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="content" > Content * </Label>

<Input id="content" {...register("content") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sessionId" > SessionId</Label>

<Input id="sessionId" {...register("sessionId") } />

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
