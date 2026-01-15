import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProjectResponsiblesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  projectId?: string;
  }

export function useProjectResponsibles(input: GetProjectResponsiblesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["projectResponsibles", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/project-responsible", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllProjectResponsibles() {
  return useQuery({
    queryKey: ["projectResponsibles", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/project-responsible", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProjectResponsible(id: string) {
  return useQuery({
    queryKey: ["projectResponsible", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/project-responsible/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProjectResponsible() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/project-responsible", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projectResponsibles"] });
    },
  });
}

export function useUpdateProjectResponsible() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/project-responsible/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["projectResponsibles"] });
      queryClient.invalidateQueries({ queryKey: ["projectResponsible", data.id] });
    },
  });
}

export function useDeleteProjectResponsible() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/project-responsible/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projectResponsibles"] });
    },
  });
}


