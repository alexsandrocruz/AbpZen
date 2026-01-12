import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

// Hook to get all specializations (not lawyer-specific)
export function useAllSpecializations() {
  return useQuery({
    queryKey: ["specializations"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/specialization", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

// Hook to get lawyer-specialization relationships for a specific lawyer
export function useLawyerSpecializations(lawyerId?: string) {
  return useQuery({
    queryKey: ["lawyerSpecializations", lawyerId],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer-specialization", {
        params: {
          lawyerId,
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
    enabled: !!lawyerId,
  });
}

export function useLawyerSpecialization(id: string) {
  return useQuery({
    queryKey: ["lawyerSpecialization", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lawyer-specialization/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lawyer-specialization", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

export function useUpdateLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lawyer-specialization/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecialization", data.id] });
    },
  });
}

export function useDeleteLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lawyer-specialization/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

// Hook to toggle a specialization for a lawyer
export function useToggleSpecialization(lawyerId?: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ specializationId, isChecked }: { specializationId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove the relationship
        const response = await apiClient.get("/api/app/lawyer-specialization", {
          params: { lawyerId, specializationId, maxResultCount: 1 },
        });
        if (response.data.items?.length > 0) {
          await apiClient.delete(`/api/app/lawyer-specialization/${response.data.items[0].id}`);
        }
      } else {
        // Add the relationship
        await apiClient.post("/api/app/lawyer-specialization", {
          lawyerId,
          specializationId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations", lawyerId] });
    },
  });
}
